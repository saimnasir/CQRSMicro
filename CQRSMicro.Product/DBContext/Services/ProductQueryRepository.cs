using CQRSMicro.Domain.DbContexts.Services;
using CQRSMicro.Product.DBContext.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CQRSMicro.Product.DBContext.Services
{
    public class ProductQueryRepository : BaseQueryRepository<Entities.Product, ProductDbContext, Guid>, IProductQueryRepository
    {
        public ProductQueryRepository(DbContextOptions<ProductDbContext> options) : base(options)
        {
        }

        protected override ProductDbContext GetContext() => new(DbOptions);

        protected override IQueryable<Entities.Product> GetDbSetWithIncludes(DbContext ctx) => ctx.Set<Entities.Product>();
    }
}