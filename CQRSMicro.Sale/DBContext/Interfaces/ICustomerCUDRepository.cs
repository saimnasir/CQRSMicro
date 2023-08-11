using Patika.Framework.Domain.Interfaces.Repository;

namespace CQRSMicro.Sale.DBContext.Interfaces
{
    public interface ICustomerCUDRepository : IGenericRepository<Entities.Customer, Guid>
    {
    }
}
