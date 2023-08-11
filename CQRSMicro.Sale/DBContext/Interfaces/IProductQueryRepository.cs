using Patika.Framework.Domain.Interfaces.Repository;

namespace CQRSMicro.Sale.DBContext.Interfaces
{
    public interface IProductQueryRepository : IGenericDapperRepository<Entities.Product, Guid>
    {
    }
}
