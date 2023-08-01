using Patika.Framework.Shared.Interfaces;

namespace CQRSMicro.Domain.DbContexts.Interfaces.Repositories
{
    public interface IBaseRepository<T, U> where T : IEntity<U> where U : struct
    {
    }
}
