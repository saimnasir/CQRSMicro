using Patika.Framework.Domain.Interfaces.Repository;

namespace CQRSMicro.Sale.DBContext.Interfaces
{
    public interface ISaleReportCUDRepository : IGenericRepository<Entities.SaleReport, Guid>
    {
    }
}
