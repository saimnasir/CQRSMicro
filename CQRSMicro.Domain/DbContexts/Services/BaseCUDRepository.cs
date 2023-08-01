using CQRSMicro.Domain.DbContexts.Interfaces.Repositories;
using CQRSMicro.Domain.DbContexts.Interfaces.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Patika.Framework.Shared.Extensions;
using Patika.Framework.Shared.Interfaces;
using System.Linq.Expressions;

namespace CQRSMicro.Domain.DbContexts.Services
{
    public abstract class BaseCUDRepository<T, TDbContext, U>
        : BaseRepository<T, TDbContext, U>, IBaseCUDRepository<T, U> where T : class,
        IEntity<U>, new() where TDbContext : DbContext where U : struct
    {
        private TDbContext GetContextWithUOW(IUnitOfWorkHostInterface? uow) => uow == null || uow.DbContext == null ? GetContext() : uow.DbContext as TDbContext;

        public BaseCUDRepository(DbContextOptions<TDbContext> options) : base(options) { }

        public BaseCUDRepository(DbContextOptions<TDbContext> options, IServiceProvider serviceProvider) : base(options, serviceProvider) { }

        public virtual async Task DeleteManyAsync(
            IEnumerable<T> entities,
            IUnitOfWorkHostInterface? unitOfWorkHost = null,
            CancellationToken cancellationToken = default)
        {
            var ctx = GetContextWithUOW(unitOfWorkHost);

            if (IsLogicalDelete)
            {
                foreach (var item in entities)
                {
                    (item as ILogicalDelete).IsDeleted = true;
                    ctx.Set<T>().Attach(item);
                    ctx.Set<T>().Update(item);
                }
            }
            else
            {
                ctx.Set<T>().RemoveRange(entities);
            }

            await ctx.SaveChangesAsync(cancellationToken);

            if (unitOfWorkHost != null)
            {
                unitOfWorkHost.Committed += async (s, e) =>
                {
                    await UpdateCache();
                };
            }
            else
            {
                await UpdateCache(ctx, cancellationToken);
                await DisposeLocalContextAsync(ctx);
            }
        }
        public virtual async Task DeleteOneAsync(
                 T item,
                 IUnitOfWorkHostInterface? unitOfWorkHost = null,
                 CancellationToken cancellationToken = default)
        {
            var ctx = GetContextWithUOW(unitOfWorkHost);

            if (IsLogicalDelete)
            {
                (item as ILogicalDelete).IsDeleted = true;
                ctx.Set<T>().Attach(item);
                ctx.Set<T>().Update(item);
            }
            else
            {
                ctx.Set<T>().Remove(item);
            }
            await ctx.SaveChangesAsync(cancellationToken);

            if (unitOfWorkHost != null)
            {
                unitOfWorkHost.Committed += async (s, e) =>
                {
                    await UpdateCache();
                };
            }
            else
            {
                await UpdateCache(ctx, cancellationToken);
                await DisposeLocalContextAsync(ctx);
            }
        }

        public virtual async Task<IEnumerable<T>> InsertManyAsync(
            IEnumerable<T> entities,
            IUnitOfWorkHostInterface? unitOfWorkHost = null,
            CancellationToken cancellationToken = default)
        {
            var ctx = GetContextWithUOW(unitOfWorkHost);

            var createDate = DateTime.UtcNow;
            foreach (var entity in entities)
            {
                entity.Id = GeneralTypeExtensions.NewId(entity.Id);
                SetUpdatedAndCreated(entity);
                SetIsActiveFlag(entity);
            }

            await ctx.Set<T>().AddRangeAsync(entities, cancellationToken);
            await ctx.SaveChangesAsync(cancellationToken);

            if (unitOfWorkHost != null)
            {
                unitOfWorkHost.Committed += async (s, e) =>
                {
                    await UpdateCache();
                };
            }
            else
            {
                await UpdateCache(ctx, cancellationToken);
                await DisposeLocalContextAsync(ctx);
            }
            return entities;
        }

        public virtual async Task<T> InsertOneAsync(
            T entity,
            IUnitOfWorkHostInterface? unitOfWorkHost = null,
            CancellationToken cancellationToken = default)
        {
            var ctx = GetContextWithUOW(unitOfWorkHost);

            entity.Id = GeneralTypeExtensions.NewId(entity.Id);
            SetUpdatedAndCreated(entity);
            SetIsActiveFlag(entity);

            await ctx.Set<T>().AddAsync(entity, cancellationToken);
            await ctx.SaveChangesAsync(cancellationToken);

            if (unitOfWorkHost != null)
            {
                unitOfWorkHost.Committed += async (s, e) =>
                {
                    await UpdateCache();
                };
            }
            else
            {
                await UpdateCache(ctx, cancellationToken);
                await DisposeLocalContextAsync(ctx);
            }
            return entity;
        }


        public virtual async Task UpdateOneAsync(
            T entity,
            IUnitOfWorkHostInterface? unitOfWorkHost = null,
            Expression<Func<T, object>>? includes = null,
            CancellationToken cancellationToken = default)
        {
            var ctx = GetContextWithUOW(unitOfWorkHost);

            var existingEntity = includes != null
                ? await ctx.Set<T>().Where(x => x.Id.Equals(entity.Id)).Include(includes).SingleOrDefaultAsync()
                : await ctx.Set<T>().Where(x => x.Id.Equals(entity.Id)).SingleOrDefaultAsync();

            var existingEntityProperties = existingEntity.GetType().GetProperties();

            foreach (var property in existingEntityProperties)
            {
                property.SetValue(existingEntity, entity.GetType().GetProperty(property.Name).GetValue(entity));
            }

            SetUpdated(existingEntity);

            ctx.Set<T>().Attach(existingEntity);
            ctx.Set<T>().Update(existingEntity);

            await ctx.SaveChangesAsync(cancellationToken);

            if (unitOfWorkHost != null)
            {
                unitOfWorkHost.Committed += async (s, e) =>
                {
                    await UpdateCache();
                };
            }
            else
            {
                await UpdateCache(ctx, cancellationToken);
                await DisposeLocalContextAsync(ctx);
            }
        }

        public virtual async Task UpdateManyAsync(
            IEnumerable<T> entities,
            IUnitOfWorkHostInterface? unitOfWorkHost = null,
            Expression<Func<T, object>>? includes = null,
            CancellationToken cancellationToken = default)
        {
            var ctx = GetContextWithUOW(unitOfWorkHost);

            var updateDate = DateTime.UtcNow;
            foreach (var entity in entities)
            {
                var existingEntity = includes != null
                    ? await ctx.Set<T>().Where(x => x.Id.Equals(entity.Id)).Include(includes).SingleOrDefaultAsync()
                    : await ctx.Set<T>().Where(x => x.Id.Equals(entity.Id)).SingleOrDefaultAsync();

                var existingEntityProperties = existingEntity.GetType().GetProperties();

                foreach (var property in existingEntityProperties)
                {
                    property.SetValue(existingEntity, entity.GetType().GetProperty(property.Name).GetValue(entity));
                }

                SetUpdated(entity);

                ctx.Set<T>().AttachRange(existingEntity);
                ctx.Set<T>().UpdateRange(existingEntity);
                await ctx.SaveChangesAsync(cancellationToken);
            }

            if (unitOfWorkHost != null)
            {
                unitOfWorkHost.Committed += async (s, e) =>
                {
                    await UpdateCache();
                };
            }
            else
            {
                await UpdateCache(ctx, cancellationToken);
                await DisposeLocalContextAsync(ctx);
            }
        }

        private async Task UpdateCache(
            TDbContext? dbContext = null,
            CancellationToken cancellationToken = default)
            => await FillCache(dbContext, cancellationToken);
        private static async Task DisposeLocalContextAsync(TDbContext ctx)
        {
            if (ctx != null)
            {
                try { await ctx.DisposeAsync(); } catch { }
            }
        }

        private void SetUpdatedAndCreated(T entity)
        {
            var utcNow = DateTime.UtcNow;
            if (entity is IHasCreated hasCreated)
            {
                hasCreated.CreatedAt = utcNow;
            }
            if (entity is IHasUpdated hasUpdated)
            {
                hasUpdated.UpdatedAt = utcNow;
            }
            if (entity is IHasCreatedBy hasCreatedBy)
            {
                hasCreatedBy.CreatedById = GetUserId();
            }
            if (entity is IHasUpdatedBy hasUpdatedBy)
            {
                hasUpdatedBy.UpdatedById = GetUserId();
            }
        }

        private void SetIsActiveFlag(T entity)
        {
            if (entity is IActiveFlag isActive)
            {
                isActive.IsActive = true;
            }
        }

        private void SetUpdated(T entity)
        {
            var utcNow = DateTime.UtcNow;
            if (entity is IHasUpdated hasUpdated)
            {
                hasUpdated.UpdatedAt = utcNow;
            }
            if (entity is IHasUpdatedBy hasUpdatedBy)
            {
                hasUpdatedBy.UpdatedById = GetUserId();
            }
        }

        private Guid? GetUserId()
        {
            if (ClientInformationService is not null && ClientInformationService?.UserId is not null)
                return ClientInformationService.UserId.ToGuid();
            return null;
        }

    }
}
