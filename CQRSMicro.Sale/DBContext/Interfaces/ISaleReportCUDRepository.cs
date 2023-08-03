using CQRSMicro.Domain.DbContexts.Interfaces.Repositories;

namespace CQRSMicro.Sale.DBContext.Interfaces
{
    public interface ISaleReportCUDRepository : IBaseCUDRepository<Entities.SaleReport, Guid>
    {
    }
}
