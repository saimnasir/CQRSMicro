using CQRSMicro.Domain.DbContexts.Interfaces.Repositories;

namespace CQRSMicro.Sale.DBContext.Interfaces
{
    public interface ISaleCUDRepository : IBaseCUDRepository<Entities.Sale, Guid>
    {
    }
}
