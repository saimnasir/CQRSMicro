using Microsoft.EntityFrameworkCore;
using Patika.Framework.Shared.Entities;

namespace CQRSMicro.Domain.DbContexts
{
    public partial class LogDbContext : DbContextWithUnitOfWork<LogDbContext>
    {
        public LogDbContext(DbContextOptions<LogDbContext> options)
            : base(options)
        {
        }

        internal DbSet<Log>? Logs { get; set; }
        internal DbSet<LogDetail>? LogDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LogDetail>()
                .HasOne(s => s.Log)
                .WithMany(g => g.Details)
                .HasForeignKey(s => s.LogId);
        }
    }
}
