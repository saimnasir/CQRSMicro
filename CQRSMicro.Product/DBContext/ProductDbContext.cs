using CQRSMicro.Domain.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace CQRSMicro.Product.DBContext
{

    public class ProductDbContext : DbContextWithUnitOfWork<ProductDbContext>
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {
        }

        public DbSet<Entities.Product> Products { get; set; } = default!;
    }
}
