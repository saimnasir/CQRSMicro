using CQRSMicro.Domain.DbContexts.Interfaces.Repositories;

namespace CQRSMicro.Sale.DBContext.Interfaces
{
    public interface IProductQueryRepository : IBaseQueryRepository<Entities.Product, Guid>
    {
    }
}
