using CQRSMicro.Domain.DbContexts.Services;
using CQRSMicro.Sale.DBContext;
using CQRSMicro.Sale.DBContext.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CQRSMicro.Sale.DBContext.Services
{
    public class SaleReportCUDRepository : BaseCUDRepository<Entities.SaleReport, SaleDbContext, Guid>, ISaleReportCUDRepository
    {
        public SaleReportCUDRepository(DbContextOptions<SaleDbContext> options) : base(options)
        {
        }

        protected override SaleDbContext GetContext() => new(DbOptions);

        protected override IQueryable<Entities.SaleReport> GetDbSetWithIncludes(DbContext ctx) => ctx.Set<Entities.SaleReport>();

    }
}