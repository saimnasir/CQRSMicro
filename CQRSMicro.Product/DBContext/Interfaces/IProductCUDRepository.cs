using CQRSMicro.Domain.DbContexts.Interfaces.Repositories;

namespace CQRSMicro.Product.DBContext.Interfaces
{
    public interface IProductCUDRepository : IBaseCUDRepository<Entities.Product, Guid>
    {
    }
}
