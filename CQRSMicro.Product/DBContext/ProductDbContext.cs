using Microsoft.EntityFrameworkCore;
using Patika.Framework.Domain.Services;

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
