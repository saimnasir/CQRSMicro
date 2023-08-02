using CQRSMicro.CustomerApp.DBContext;
using CQRSMicro.CustomerApp.DBContext.Interfaces;
using CQRSMicro.Domain.DbContexts.Services;
using Microsoft.EntityFrameworkCore;

namespace CQRSMicro.CustomerApp.DBContext.Services
{
    public class CustomerCUDRepository : BaseCUDRepository<Entities.Customer, CustomerDbContext, Guid>, ICustomerCUDRepository
    {
        public CustomerCUDRepository(DbContextOptions<CustomerDbContext> options) : base(options)
        {
        }

        protected override CustomerDbContext GetContext() => new(DbOptions);

        protected override IQueryable<Entities.Customer> GetDbSetWithIncludes(DbContext ctx) => ctx.Set<Entities.Customer>();

    }
}