using CQRSMicro.Domain.DbContexts.Interfaces.Repositories;

namespace CQRSMicro.CustomerApp.DBContext.Interfaces
{
    public interface ICustomerCUDRepository : IBaseCUDRepository<Entities.Customer, Guid>
    {
    }
}
