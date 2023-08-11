using Patika.Framework.Domain.Interfaces.Repository;

namespace CQRSMicro.Sale.DBContext.Interfaces
{
    public interface IProductCUDRepository : IGenericRepository<Entities.Product, Guid>
    {
    }
}
