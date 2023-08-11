using Microsoft.EntityFrameworkCore;

using Patika.Framework.Domain.Services;
namespace CQRSMicro.Sale.DBContext
{

    public class SaleDbContext : DbContextWithUnitOfWork<SaleDbContext>
    {
        public SaleDbContext(DbContextOptions<SaleDbContext> options) : base(options)
        {
        }

        public DbSet<Entities.Sale> Sales { get; set; } = default!;
        public DbSet<Entities.Product> Products { get; set; } = default!;
        public DbSet<Entities.Customer> Customers { get; set; } = default!;
        public DbSet<Entities.SaleReport> SaleReports { get; set; } = default!;
    }
}
