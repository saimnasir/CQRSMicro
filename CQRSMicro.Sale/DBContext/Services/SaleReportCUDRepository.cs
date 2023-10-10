using CQRSMicro.Sale.DBContext.Interfaces;
using Microsoft.EntityFrameworkCore;
using Patika.Framework.Domain.Services;

namespace CQRSMicro.Sale.DBContext.Services
{
    public class SaleReportCUDRepository : GenericRepository<Entities.SaleReport, SaleDbContext, Guid>, ISaleReportCUDRepository
    {
        public SaleReportCUDRepository(DbContextOptions<SaleDbContext> options) : base(options)
        {
        }

        protected override SaleDbContext GetContext() => new(DbOptions);

        protected override IQueryable<Entities.SaleReport> GetDbSetWithIncludes(SaleDbContext ctx) => ctx.Set<Entities.SaleReport>();

    }
}