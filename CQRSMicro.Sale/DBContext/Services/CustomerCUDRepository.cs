using CQRSMicro.Domain.DbContexts.Services;
using CQRSMicro.Sale.DBContext;
using CQRSMicro.Sale.DBContext.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CQRSMicro.Sale.DBContext.Services
{
    public class CustomerCUDRepository : BaseCUDRepository<Entities.Customer, SaleDbContext, Guid>, ICustomerCUDRepository
    {
        public CustomerCUDRepository(DbContextOptions<SaleDbContext> options) : base(options)
        {
        }

        protected override SaleDbContext GetContext() => new(DbOptions);

        protected override IQueryable<Entities.Customer> GetDbSetWithIncludes(DbContext ctx) => ctx.Set<Entities.Customer>();

    }
}