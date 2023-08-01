using CQRSMicro.Domain.DbContexts.Interfaces.Repositories;

namespace CQRSMicro.Product.DBContext.Interfaces
{
    public interface IProductQueryRepository : IBaseQueryRepository<Entities.Product, Guid>
    {
    }
}
