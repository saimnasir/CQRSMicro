using Patika.Framework.Domain.Interfaces.Repository;

namespace CQRSMicro.Sale.DBContext.Interfaces
{
    public interface ISaleQueryRepository : IGenericQueryRepository<Entities.Sale, Guid>
    {
    }
}
