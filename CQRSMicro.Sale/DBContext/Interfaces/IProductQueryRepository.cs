using Patika.Framework.Domain.Interfaces.Repository;

namespace CQRSMicro.Sale.DBContext.Interfaces
{
    public interface IProductQueryRepository : IGenericQueryRepository<Entities.Product, Guid>
    {
    }
}
