using CQRSMicro.Domain.DbContexts.Interfaces.Repositories;

namespace CQRSMicro.Sale.DBContext.Interfaces
{
    public interface ISaleQueryRepository : IBaseQueryRepository<Entities.Sale, Guid>
    {
    }
}
