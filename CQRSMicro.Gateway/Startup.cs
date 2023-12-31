using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace Koctas.PublicGateway
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private IWebHostEnvironment Environment { get; }
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore().AddApiExplorer();
            services.AddOcelot();
            services.AddSwaggerForOcelot(Configuration);

            services.AddCors(opts =>
            {
                opts.AddDefaultPolicy(policy =>
                {
                    policy.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost");
                    policy.WithOrigins("http://localhost/*", "https://localhost/*")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", context =>
                {
                    context.Response.Redirect("/swagger");
                    return Task.CompletedTask;
                });
            });

            app.UseSwaggerForOcelotUI(opt =>
            {
                opt.PathToSwaggerGenerator = "/swagger/docs";
            });
            await app.UseOcelot();
        }
    }
}
