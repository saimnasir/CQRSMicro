using CQRSMicro.Domain.DbContexts.Interfaces.Repositories;

namespace CQRSMicro.CustomerApp.DBContext.Interfaces
{
    public interface ICustomerQueryRepository : IBaseQueryRepository<Entities.Customer, Guid>
    {
    }
}
