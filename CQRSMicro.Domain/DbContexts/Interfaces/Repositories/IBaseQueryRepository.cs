using Patika.Framework.Shared.Entities;
using Patika.Framework.Shared.Interfaces;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace CQRSMicro.Domain.DbContexts.Interfaces.Repositories
{
    public interface IBaseQueryRepository<T, U> where T : IEntity<U> where U : struct
    {
        Task<PagedResult<T>> GetAllAsync(Pagination? pagination = null, List<Sort>? sorts = null, bool includeInActives = false, bool includeLogicalDeleted = false, CancellationToken cancellationToken = default);

        Task<PagedResult<T>> WhereAsync(Expression<Func<T, bool>>? selector, bool includeChilds = false, Pagination? pagination = null, List<Sort>? sorts = null, bool includeInActives = false, bool includeLogicalDeleted = false, CancellationToken cancellationToken = default);

        Task<PagedResult<T>> WhereAsync(IEnumerable<Condition> conditions, bool includeChilds = false, Pagination pagination = null, List<Sort> sorts = null, bool includeInActives = false, bool includeLogicalDeleted = false, CancellationToken cancellationToken = default);

        Task<T?> GetByIdAsync(U id, bool includeChilds = false, bool includeInActives = false, bool includeLogicalDeleted = false, CancellationToken cancellationToken = default);

        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? selector = null, bool includeChilds = false, bool includeInActives = false, bool includeLogicalDeleted = false, CancellationToken cancellationToken = default);

        Task<T?> LastOrDefaultAsync(Expression<Func<T, bool>>? selector = null, bool includeChilds = false, bool includeInActives = false, bool includeLogicalDeleted = false, CancellationToken cancellationToken = default);

        Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>>? selector = null, bool includeChilds = false, bool includeInActives = false, bool includeLogicalDeleted = false, CancellationToken cancellationToken = default);

        Task<T> SingleAsync(Expression<Func<T, bool>>? selector = null, bool includeChilds = false, bool includeInActives = false, bool includeLogicalDeleted = false, CancellationToken cancellationToken = default);

        Task<int> CountAsync(Expression<Func<T, bool>>? selector = null, bool includeInActives = false, bool includeLogicalDeleted = false, CancellationToken cancellationToken = default);

        Task<bool> AnyAsync(Expression<Func<T, bool>>? selector = null, bool includeInActives = false, bool includeLogicalDeleted = false, CancellationToken cancellationToken = default);

        void SetMaxSelectCount(int count);

        Task ResetCacheAsync();
    }
}
