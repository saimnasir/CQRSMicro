using CQRSMicro.Domain.DbContexts.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Patika.Framework.Shared.Entities;
using Patika.Framework.Shared.Enums;
using Patika.Framework.Shared.Extensions;
using Patika.Framework.Shared.Interfaces;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text.Json;
using Condition = Patika.Framework.Shared.Entities.Condition;

namespace CQRSMicro.Domain.DbContexts.Services
{
    public abstract class BaseQueryRepository<T, TDbContext, U>
        : BaseRepository<T, TDbContext, U>, IBaseQueryRepository<T, U> where T : class,
        IEntity<U>, new() where TDbContext : DbContext where U : struct
    {

        public BaseQueryRepository(DbContextOptions<TDbContext> options) : base(options) { }

        public BaseQueryRepository(DbContextOptions<TDbContext> options, IServiceProvider serviceProvider) : base(options, serviceProvider) { }

        public virtual async Task<bool> AnyAsync(
            Expression<Func<T, bool>>? selector = null,
            bool includeInActives = false,
            bool includeLogicalDeleted = false,
            CancellationToken cancellationToken = default)
        {
            if (!IsCachable)
            {
                using var ctx = GetContext();

                return selector == null
                    ? await GetWithIncludeFlag(includeChilds: false, ctx, includeInActives, includeLogicalDeleted)
                        .AnyAsync(cancellationToken)
                    : await GetWithIncludeFlag(includeChilds: false, ctx, includeInActives, includeLogicalDeleted)
                        .AnyAsync(selector, cancellationToken);
            }

            var query = await GetCacheableQueryAsync(includeInActives, includeLogicalDeleted, cancellationToken);
            return selector == null
                ? await query.AnyAsync(cancellationToken)
                : await query.AnyAsync(selector, cancellationToken);
        }

        public virtual async Task<int> CountAsync(
                Expression<Func<T, bool>>? selector = null,
                bool includeInActives = false,
                bool includeLogicalDeleted = false,
                CancellationToken cancellationToken = default)
        {
            if (!IsCachable)
            {
                using var ctx = GetContext();
                return selector == null
                    ? await GetWithIncludeFlag(includeChilds: false, ctx, includeInActives, includeLogicalDeleted)
                        .CountAsync(cancellationToken)
                    : await GetWithIncludeFlag(includeChilds: false, ctx, includeInActives, includeLogicalDeleted)
                        .CountAsync(selector, cancellationToken);
            }

            var query = await GetCacheableQueryAsync(includeInActives, includeLogicalDeleted, cancellationToken);
            return selector == null
                ? await query.CountAsync(cancellationToken)
                : await query.CountAsync(selector, cancellationToken);
        }

        public virtual async Task<T?> FirstOrDefaultAsync(
            Expression<Func<T, bool>>? selector = null,
            bool includeChilds = false,
            bool includeInActives = false,
            bool includeLogicalDeleted = false,
            CancellationToken cancellationToken = default)
        {
            if (!IsCachable)
            {
                using var ctx = GetContext();
                return await GetWithIncludeFlag(includeChilds, ctx, includeInActives, includeLogicalDeleted)
                    .FirstOrDefaultAsync(selector, cancellationToken);
            }

            var query = await GetCacheableQueryAsync(includeInActives, includeLogicalDeleted, cancellationToken);
            return selector == null
                ? query.FirstOrDefault()
                : query.FirstOrDefault(selector.Compile());
        }

        public virtual async Task<PagedResult<T>> GetAllAsync(
              Pagination? pagination = default,
              List<Sort>? sorts = default,
              bool includeInActives = false,
              bool includeLogicalDeleted = false,
              CancellationToken cancellationToken = default)
        {
            if (!IsCachable)
            {
                using var ctx = GetContext();
                return await GetWithIncludeFlag(includeChilds: false, ctx, includeInActives, includeLogicalDeleted)
                    .SortBy(sorts)
                    .PaginateAsync(pagination, DefaultMaxCountForSelect);
            }

            var query = await GetCacheableQueryAsync(includeInActives, includeLogicalDeleted, cancellationToken);
            return await query
                .SortBy(sorts)
                .PaginateAsync(pagination, DefaultMaxCountForSelect);
        }

        public virtual async Task<T?> GetByIdAsync(
                 U id, bool includeChilds = false,
                 bool includeInActives = false,
                 bool includeLogicalDeleted = false,
                 CancellationToken cancellationToken = default)
                 => await SingleOrDefaultAsync(x => x.Id.Equals(id), includeChilds, includeInActives, includeLogicalDeleted, cancellationToken);

        public virtual async Task<T?> LastOrDefaultAsync(
                 Expression<Func<T, bool>>? selector = null,
                 bool includeChilds = false,
                 bool includeInActives = false,
                 bool includeLogicalDeleted = false,
                 CancellationToken cancellationToken = default)
        {
            if (!IsCachable)
            {
                using var ctx = GetContext();
                return await GetWithIncludeFlag(includeChilds, ctx, includeInActives, includeLogicalDeleted)
                    .LastOrDefaultAsync(selector, cancellationToken);
            }
            var query = await GetCacheableQueryAsync(includeInActives, includeLogicalDeleted, cancellationToken);

            return selector == null
                ? query.LastOrDefault()
                : query.LastOrDefault(selector.Compile());
        }


        public void SetMaxSelectCount(int count)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException("Must be bigger than zero");
            }
            DefaultMaxCountForSelect = count;
        }

        public virtual async Task<T> SingleAsync(
               Expression<Func<T, bool>>? selector = null,
               bool includeChilds = false,
               bool includeInActives = false,
               bool includeLogicalDeleted = false,
               CancellationToken cancellationToken = default)
        {
            if (!IsCachable)
            {
                using var ctx = GetContext();
                return await GetWithIncludeFlag(includeChilds, ctx, includeInActives, includeLogicalDeleted)
                    .SingleAsync(selector, cancellationToken);
            }
            var query = await GetCacheableQueryAsync(includeInActives, includeLogicalDeleted, cancellationToken);

            return selector == null
                ? query.Single()
                : query.Single(selector.Compile());
        }

        public virtual async Task<T?> SingleOrDefaultAsync(
                 Expression<Func<T, bool>>? selector = null,
                 bool includeChilds = false,
                 bool includeInActives = false,
                 bool includeLogicalDeleted = false,
                 CancellationToken cancellationToken = default)
        {
            if (!IsCachable)
            {
                using var ctx = GetContext();
                return await GetWithIncludeFlag(includeChilds, ctx, includeInActives, includeLogicalDeleted)
                    .SingleOrDefaultAsync(selector, cancellationToken);
            }
            var query = await GetCacheableQueryAsync(includeInActives, includeLogicalDeleted, cancellationToken);

            return selector == null
                ? query.SingleOrDefault()
                : query.SingleOrDefault(selector.Compile());
        }

        public virtual async Task<PagedResult<T>> WhereAsync(
         Expression<Func<T, bool>>? selector = null,
         bool includeChilds = false,
         Pagination? pagination = null,
         List<Sort>? sorts = null,
         bool includeInActives = false,
         bool includeLogicalDeleted = false,
         CancellationToken cancellationToken = default)
        {
            if (!IsCachable)
            {
                using var ctx = GetContext();
                return await GetWithIncludeFlag(includeChilds, ctx, includeInActives, includeLogicalDeleted)
                    .Where(selector)
                    .SortBy(sorts)
                    .PaginateAsync(pagination, DefaultMaxCountForSelect);
            }

            var query = await GetCacheableQueryAsync(includeInActives, includeLogicalDeleted, cancellationToken);
            return await query
                .SortBy(sorts)
                .PaginateAsync(pagination, DefaultMaxCountForSelect);
        }

        public virtual async Task<PagedResult<T>> WhereAsync(
           IEnumerable<Condition> conditions,
           bool includeChilds = false,
           Pagination? pagination = null,
           List<Sort>? sorts = null,
           bool includeInActives = false,
           bool includeLogicalDeleted = false,
           CancellationToken cancellationToken = default)
        {
            if (!IsCachable)
            {
                using var ctx = GetContext();
                return await GetWithIncludeFlag(includeChilds, ctx, includeInActives, includeLogicalDeleted)
                    .Where(conditions)
                    .SortBy(sorts)
                    .PaginateAsync(pagination, DefaultMaxCountForSelect);
            }

            var query = await GetCacheableQueryAsync(includeInActives, includeLogicalDeleted, cancellationToken);
            return await query
                .Where(conditions)
                .SortBy(sorts)
                .PaginateAsync(pagination, DefaultMaxCountForSelect);
        }

        private IQueryable<T> GetWithIncludeFlag(bool includeChilds, TDbContext ctx, bool includeInActives, bool includeLogicalDeleted)
        {
            var query = includeChilds ? GetDbSetWithIncludes(ctx) : ctx.Set<T>();

            query = query
                .WhereIf(HasActiveFlag && !includeInActives, IsActiveWhereClause)
                .WhereIf(IsLogicalDelete && !includeLogicalDeleted, IsNotLogicalDeletedWhereClause);
            query = ApplyTenantFilter(query);
            return query;
        }
        private async Task<IQueryable<T>> GetCacheableQueryAsync(bool includeInActives, bool includeLogicalDeleted, CancellationToken cancellationToken = default)
        {
            var cachedData = await GetCachedData(cancellationToken);
            var lst = JsonSerializer.Deserialize<IEnumerable<T>>(cachedData.ToString());

            var query = lst.AsQueryable()
                .WhereIf(HasActiveFlag && !includeInActives, IsActiveWhereClause)
                .WhereIf(IsLogicalDelete && !includeLogicalDeleted, IsNotLogicalDeletedWhereClause);
            query = ApplyTenantFilter(query);
            return query;
        }
        private IQueryable<T> ApplyTenantFilter(IQueryable<T> query)
        {
            if (!IsMultiTenant)
                return query;
            if (TenantService is null)
                return query;
            if (TenantService.IgnoreTenancy)
                return query;
            var condition = new Condition
            {
                PropertyName = nameof(IMultiTenant.TenantId),
                Operator = ConditionOperatorEnum.Equal,
                Values = new List<string> { TenantService.Tenant.ToString() }
            };
            return query.Where(condition);
        }

        private async Task<string> GetCachedData(CancellationToken cancellationToken)
        {
            var json = await CacheDb.StringGetAsync(CacheKey);
            if (string.IsNullOrEmpty(json))
            {
                await FillCache(cancellationToken: cancellationToken);
                json = await CacheDb.StringGetAsync(CacheKey);
            }

            return json.ToString();
        }
    }
}
