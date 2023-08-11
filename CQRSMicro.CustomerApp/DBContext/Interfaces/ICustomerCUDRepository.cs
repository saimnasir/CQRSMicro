using Patika.Framework.Domain.Interfaces.Repository;

namespace CQRSMicro.CustomerApp.DBContext.Interfaces
{
    public interface ICustomerCUDRepository : IGenericRepository<Entities.Customer, Guid>
    {
    }
}
