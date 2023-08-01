using CQRSMicro.Domain.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace CQRSMicro.Sale.DBContext
{

    public class SaleDbContext : DbContextWithUnitOfWork<SaleDbContext>
    {
        public SaleDbContext(DbContextOptions<SaleDbContext> options) : base(options)
        {
        }

        public DbSet<Entities.Sale> Sales { get; set; } = default!;
        public DbSet<Entities.Product> Products { get; set; } = default!;
    }
}
