using CQRSMicro.Product.DBContext.Interfaces;
using Microsoft.EntityFrameworkCore;
using Patika.Framework.Domain.Services;

namespace CQRSMicro.Product.DBContext.Services
{
    public class ProductCUDRepository : GenericRepository<Entities.Product, ProductDbContext, Guid>, IProductCUDRepository
    {
        public ProductCUDRepository(DbContextOptions<ProductDbContext> options) : base(options)
        {
        }

        protected override ProductDbContext GetContext() => new(DbOptions);

        protected override IQueryable<Entities.Product> GetDbSetWithIncludes(DbContext ctx) => ctx.Set<Entities.Product>();

    }
}