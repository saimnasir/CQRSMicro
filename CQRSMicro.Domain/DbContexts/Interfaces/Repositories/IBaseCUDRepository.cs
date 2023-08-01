using CQRSMicro.Domain.DbContexts.Interfaces.UnitOfWork;
using Patika.Framework.Shared.Interfaces;
using System.Linq.Expressions;

namespace CQRSMicro.Domain.DbContexts.Interfaces.Repositories
{
    public interface IBaseCUDRepository<T, U> where T : IEntity<U> where U : struct
    {
        Task<T> InsertOneAsync(T entity, IUnitOfWorkHostInterface? unitOfWorkHost = null, CancellationToken cancellationToken = default);

        Task<IEnumerable<T>> InsertManyAsync(IEnumerable<T> entities, IUnitOfWorkHostInterface? unitOfWorkHost = null, CancellationToken cancellationToken = default);

        Task UpdateOneAsync(T entity, IUnitOfWorkHostInterface? unitOfWorkHost = null, Expression<Func<T, object>>? includes = null, CancellationToken cancellationToken = default);

        Task UpdateManyAsync(IEnumerable<T> entities, IUnitOfWorkHostInterface? unitOfWorkHost = null, Expression<Func<T, object>>? includes = null, CancellationToken cancellationToken = default);

        Task DeleteOneAsync(T entity, IUnitOfWorkHostInterface? unitOfWorkHost = null, CancellationToken cancellationToken = default);

        Task DeleteManyAsync(IEnumerable<T> entities, IUnitOfWorkHostInterface? unitOfWorkHost = null, CancellationToken cancellationToken = default);
    }
}
