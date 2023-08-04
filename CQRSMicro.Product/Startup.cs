﻿using Microsoft.AspNetCore.HttpOverrides;
using Patika.Framework.Shared.Consts;
using Patika.Framework.Shared.Entities;
using Patika.Framework.Shared.Extensions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Newtonsoft.Json.Converters;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Patika.Framework.Shared.Events;
using Patika.Framework.Shared.Interfaces;
using Patika.Framework.Shared.Services;
using Patika.Framework.Utilities.Queue.Extensions;
using Patika.Framework.Utilities.Queue.Interfaces;
using Patika.Framework.Utilities.Queue.Services;
using CQRSMicro.Domain.Logger;
using CQRSMicro.Domain.DbContexts;
using CQRSMicro.Domain.DbContexts.Interfaces.UnitOfWork;
using CQRSMicro.Product.DBContext;
using CQRSMicro.Product.DBContext.Interfaces;
using CQRSMicro.Product.DBContext.Services;
using CQRSMicro.Product.CQRS.Handlers;
using CQRSMicro.Domain.Models;
using CQRSMicro.Product.QueConsumers;

namespace CQRSMicro.Product
{
    public class Startup
    {
        public static ClientAuthenticationParams AuthenticationParams { get; private set; } = new ClientAuthenticationParams();
        public IConfiguration Configuration { get; }
        public Configuration AppConfiguration { get; set; } = new();
        private IWebHostEnvironment Environment { get; }
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore().AddApiExplorer();

            AddServices(services);
            SetupCORS(services);
        }
        private void AddServices(IServiceCollection services)
        {
            AddConfigurations(services);
            AddDatabases(services);
            AddApplicationServices(services);
            AddRepositories(services);
            services.AddControllers();
            services
              .AddControllersWithViews()
              .AddNewtonsoftJson(options => options.SerializerSettings.Converters.Add(new StringEnumConverter()));

            services.AddRouting(o => o.LowercaseUrls = true);
            AddSwagger(services);

            //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());                        
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            AddQueueServices(services);
        }
        private void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGenNewtonsoftSupport();

            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    BearerFormat = "JWT",
                    Name = "Authorization",
                    Description = "Example: \"Bearer {token}\"",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
                c.DocInclusionPredicate((name, api) => true);
                c.TagActionsBy(api =>
                {
                    if (api.GroupName != null)
                    {
                        return new[] { api.GroupName };
                    }

                    if (api.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
                    {
                        return new[] { controllerActionDescriptor.ControllerName };
                    }

                    throw new InvalidOperationException("Unable to determine tag for endpoint.");
                });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Product API",
                    Description = "Product App Web Server API"
                });
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
        }
        private void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<ILogRepository, LogRepository>();
            services.AddScoped<ILogWriter, LogWriter>();

            services.AddScoped<IProductCUDRepository, ProductCUDRepository>();
            services.AddScoped<IProductQueryRepository, ProductQueryRepository>();
        }

        private void AddDatabases(IServiceCollection services)
        {
            services.AddDbContextPool<ProductDbContext>((sp, opt) =>
            {
                var connectionString = sp.GetService<Configuration>()?.RDBMSConnectionStrings.Single(m => m.Name.Equals(DbConnectionNames.Main)).FullConnectionString ?? "";
                opt.UseMySQL(connectionString);
            }, poolSize: 4);

            services.AddDbContextPool<LogDbContext>((sp, opt) =>
            {
                var connectionString = sp.GetService<Configuration>()?.RDBMSConnectionStrings.Single(m => m.Name.Equals(DbConnectionNames.Log)).FullConnectionString ?? "";
                opt.UseMySQL(connectionString);
            }, poolSize: 4);

            services.AddScoped<IUnitOfWorkHostWithInterface, ProductDbContext>();
        }
        private static void AddApplicationServices(IServiceCollection services)
        {
            services.AddScoped<IClientInformationService, ClientInformationService>();
            //services.AddTransient<GetAllProductQueryHandler>();
            //services.AddTransient<GetByIdProductQueryHandler>();
            services.AddTransient<CreateProductCommandHandler>();
            //services.AddTransient<DeleteProductCommandHandler>();
        }
        private void AddQueueServices(IServiceCollection services)
        {
            var config = new Patika.Framework.Utilities.Queue.Models.Configuration();
            Configuration.GetSection("QueueConfiguration").Bind(config);
            services.AddQueue(config);

            services.AddScoped<IProducerService<Guid>, ProducerService<Guid>>();
            services.AddTransient<IConsumerService<ProductSoldModel>, ProductSoldConsumer>();
        }
        private void AddConfigurations(IServiceCollection services)
        {
            LogWriterExtensions.ApplicationName = AppConfiguration.ApplicationName;
            AuthenticationParams = new ClientAuthenticationParams
            {
                AuthServer = AppConfiguration.AuthServerUrl,
                ClientId = AppConfiguration.ClientId,
                ClientSecret = AppConfiguration.ClientSecret
            };

            services.AddSingleton(AuthenticationParams);
            AddAppConfiguration(services);

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
        }

        private void AddAppConfiguration(IServiceCollection services)
        {
            AppConfiguration = new Configuration();
            Configuration.GetSection("Configuration").Bind(AppConfiguration);
            services.AddSingleton(AppConfiguration);
        }

        private static void SetupCORS(IServiceCollection services)
        {
            services.AddCors(opts =>
            {
                opts.AddPolicy("corsapp", policy =>
                {
                    policy.WithOrigins("*")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    //.AllowCredentials()
                    ;
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            //if (env.IsDevelopment())
            //{
            app.UseDeveloperExceptionPage();
            //}

            app.UseForwardedHeaders();

            ConfigurationEvents.NewConfiguration(app.ApplicationServices.GetRequiredService<Configuration>());

            UseSwagger(app, env);

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("corsapp");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapDefaultControllerRoute();
            });
        }

        private static void UseSwagger(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Product Api");
                options.RoutePrefix = string.Empty;
            });

        }
    }
}
