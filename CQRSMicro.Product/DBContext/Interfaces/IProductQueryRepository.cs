
using Patika.Framework.Domain.Interfaces.Repository;

namespace CQRSMicro.Product.DBContext.Interfaces
{
    public interface IProductQueryRepository : IGenericDapperRepository<Entities.Product, Guid>
    {
    }
}
