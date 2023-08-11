using CQRSMicro.Sale.DBContext.Interfaces;
using Microsoft.EntityFrameworkCore;
using Patika.Framework.Domain.Services;

namespace CQRSMicro.Sale.DBContext.Services
{
    public class CustomerCUDRepository : GenericRepository<Entities.Customer, SaleDbContext, Guid>, ICustomerCUDRepository
    {
        public CustomerCUDRepository(DbContextOptions<SaleDbContext> options) : base(options)
        {
        }

        protected override SaleDbContext GetContext() => new(DbOptions);

        protected override IQueryable<Entities.Customer> GetDbSetWithIncludes(DbContext ctx) => ctx.Set<Entities.Customer>();

    }
}