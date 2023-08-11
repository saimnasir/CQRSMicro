using Patika.Framework.Domain.Interfaces.Repository;

namespace CQRSMicro.Product.DBContext.Interfaces
{
    public interface IProductCUDRepository : IGenericRepository<Entities.Product, Guid>
    {
    }
}
