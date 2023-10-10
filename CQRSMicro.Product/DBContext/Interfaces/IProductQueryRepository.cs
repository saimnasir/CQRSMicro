
using Patika.Framework.Domain.Interfaces.Repository;
using Patika.Framework.Domain.Services;

namespace CQRSMicro.Product.DBContext.Interfaces
{
    public interface IProductQueryRepository : IGenericQueryRepository<Entities.Product, Guid>
    {
    }
}
