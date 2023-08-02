using CQRSMicro.Domain.DbContexts.Interfaces.Repositories;

namespace CQRSMicro.Sale.DBContext.Interfaces
{
    public interface ICustomerCUDRepository : IBaseCUDRepository<Entities.Customer, Guid>
    {
    }
}
