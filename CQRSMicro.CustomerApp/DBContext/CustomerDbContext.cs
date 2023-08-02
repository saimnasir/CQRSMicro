using CQRSMicro.Domain.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace CQRSMicro.CustomerApp.DBContext
{

    public class CustomerDbContext : DbContextWithUnitOfWork<CustomerDbContext>
    {
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options)
        {
        }

        public DbSet<Entities.Customer> Customers { get; set; } = default!;
    }
}
