using Patika.Framework.Domain.Interfaces.Repository;

namespace CQRSMicro.Sale.DBContext.Interfaces
{
    public interface ISaleQueryRepository : IGenericDapperRepository<Entities.Sale, Guid>
    {
    }
}
