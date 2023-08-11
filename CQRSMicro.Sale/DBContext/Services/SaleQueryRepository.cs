using CQRSMicro.Sale.DBContext.Interfaces;
using Dapper;
using Patika.Framework.Domain.Services;
using Patika.Framework.Shared.Extensions;
using Patika.Framework.Shared.Services;
using System.Data;

namespace CQRSMicro.Sale.DBContext.Services
{
    public class SaleQueryRepository : GenericDapperRepository<Entities.Sale, Guid>, ISaleQueryRepository
    {
        public SaleQueryRepository(string connectionString, IServiceProvider serviceProvider) : base(connectionString, serviceProvider)
        {
        }

        protected override PagedResult<Entities.Sale> GetPagedResultWithIncludes(IDbConnection dbConnection) => dbConnection.GetPagedResult<Entities.Sale>(
                                                                                                               SqlQueryBuilderGenerator
                                                                                                               .GenerateQueryBuilder<Entities.Sale>()
                                                                                                               .PaginateQuery(null, 10)
                                                                                                               .ToString()
                                                                                                           ).GetAwaiter().GetResult();

        protected override IQueryable<Entities.Sale> GetQueryWithIncludes(IDbConnection dbConnection) => dbConnection.Query<Entities.Sale>(SqlQueryBuilderGenerator.GenerateQueryBuilder<Entities.Sale>().ToString()).AsQueryable();

    }
}