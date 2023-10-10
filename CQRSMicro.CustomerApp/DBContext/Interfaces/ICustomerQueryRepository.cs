using Patika.Framework.Domain.Interfaces.Repository;

namespace CQRSMicro.CustomerApp.DBContext.Interfaces
{
    public interface ICustomerQueryRepository : IGenericQueryRepository<Entities.Customer, Guid>
    {
    }
}
