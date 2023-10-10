using CQRSMicro.Sale.DBContext.Interfaces;
using Microsoft.EntityFrameworkCore;
using Patika.Framework.Domain.Services;

namespace CQRSMicro.Sale.DBContext.Services
{
    public class ProductCUDRepository : GenericRepository<Entities.Product, SaleDbContext, Guid>, IProductCUDRepository
    {
        public ProductCUDRepository(DbContextOptions<SaleDbContext> options) : base(options)
        {
        }

        protected override SaleDbContext GetContext() => new(DbOptions);

        protected override IQueryable<Entities.Product> GetDbSetWithIncludes(SaleDbContext ctx) => ctx.Set<Entities.Product>();

    }
}