using CQRSMicro.Domain.DbContexts.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Patika.Framework.Shared.Exceptions;
using Patika.Framework.Shared.Interfaces;
using Patika.Framework.Shared.Services;
using StackExchange.Redis;
using System.Text.Json;

namespace CQRSMicro.Domain.DbContexts.Services
{
    public abstract class BaseRepository<T, TDbContext, U>
        : IBaseRepository<T, U> where T : class,
        IEntity<U>, new() where TDbContext : DbContext where U : struct
    {
        protected IClientInformationService? ClientInformationService { get; }
        protected IServiceProvider? ServiceProvider { get; }
        protected ITenantService? TenantService { get; }
        protected DbContextOptions<TDbContext> DbOptions { get; }
        protected const string IsNotLogicalDeletedWhereClause = $"{nameof(ILogicalDelete.IsDeleted)} = false";
        protected const string IsActiveWhereClause = $"{nameof(IActiveFlag.IsActive)} = true";
        protected bool IsLogicalDelete { get; }
        protected bool IsMultiTenant { get; }
        protected bool IsCachable { get; }
        protected int DefaultMaxCountForSelect { get; set; } = 200;
        protected string CacheKey { get; } = string.Empty;
        protected bool HasActiveFlag { get; }
        protected TimeSpan? CacheTimeout { get; }
        protected static IDatabase CacheDb => RedisConnectorHelper.Db;
        protected abstract IQueryable<T> GetDbSetWithIncludes(DbContext ctx);
        protected abstract TDbContext GetContext();

        public BaseRepository(DbContextOptions<TDbContext> options)
        {
            DbOptions = options;
            IsLogicalDelete = typeof(T).GetInterface($"{typeof(ILogicalDelete).FullName}", true) != null;
            HasActiveFlag = typeof(T).GetInterface($"{typeof(IActiveFlag).FullName}", true) != null;
            IsCachable = typeof(T).GetInterface(typeof(ICachableEntity).Name, true) != null;
            if (IsCachable)
            {
                CacheKey = ((ICachableEntity)new T()).GetCacheKey();
                CacheTimeout = ((ICachableEntity)new T()).GetExpireTime();
            }
        }

        public BaseRepository(DbContextOptions<TDbContext> options, IServiceProvider serviceProvider) : this(options)
        {
            ServiceProvider = serviceProvider;
            IsMultiTenant = typeof(T).GetInterface($"{typeof(IMultiTenant).FullName}", true) != null;
            TenantService = ServiceProvider.GetService<ITenantService>() ?? throw new ServiceNotInjectedException($"{typeof(ITenantService).FullName}");
        }

        public async Task ResetCacheAsync()
        {
            await FillCache();
        }

        protected async Task FillCache(TDbContext? dbContext = null, CancellationToken cancellationToken = default)
        {
            if (!IsCachable) return;
            var ctx = dbContext ?? GetContext();
            var serializedData = JsonSerializer.Serialize(await ctx.Set<T>().ToListAsync(cancellationToken));
            await CacheDb.StringSetAsync(CacheKey, serializedData, CacheTimeout);
        }

    }
}
