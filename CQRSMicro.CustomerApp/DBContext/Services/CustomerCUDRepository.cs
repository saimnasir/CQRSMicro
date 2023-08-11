using CQRSMicro.CustomerApp.DBContext.Interfaces;
using Microsoft.EntityFrameworkCore;
using Patika.Framework.Domain.Services;

namespace CQRSMicro.CustomerApp.DBContext.Services
{
    public class CustomerCUDRepository : GenericRepository<Entities.Customer, CustomerDbContext, Guid>, ICustomerCUDRepository
    {
        public CustomerCUDRepository(DbContextOptions<CustomerDbContext> options) : base(options)
        {
        }

        protected override CustomerDbContext GetContext() => new(DbOptions);

        protected override IQueryable<Entities.Customer> GetDbSetWithIncludes(DbContext ctx) => ctx.Set<Entities.Customer>();

    }
}