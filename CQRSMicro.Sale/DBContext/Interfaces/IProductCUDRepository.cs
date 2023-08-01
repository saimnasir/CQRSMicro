using CQRSMicro.Domain.DbContexts.Interfaces.Repositories;

namespace CQRSMicro.Sale.DBContext.Interfaces
{
    public interface IProductCUDRepository : IBaseCUDRepository<Entities.Product, Guid>
    {
    }
}
