using CQRSMicro.Domain.DbContexts.Services;
using CQRSMicro.Sale.DBContext;
using CQRSMicro.Sale.DBContext.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CQRSMicro.Sale.DBContext.Services
{
    public class SaleQueryRepository : BaseQueryRepository<Entities.Sale, SaleDbContext, Guid>, ISaleQueryRepository
    {
        public SaleQueryRepository(DbContextOptions<SaleDbContext> options) : base(options)
        {
        }

        protected override SaleDbContext GetContext() => new(DbOptions);

        protected override IQueryable<Entities.Sale> GetDbSetWithIncludes(DbContext ctx) => ctx.Set<Entities.Sale>()
            .Include(s => s.Products)
            .Include(s => s.Customer);
    }
}