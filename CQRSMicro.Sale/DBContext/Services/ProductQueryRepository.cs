using CQRSMicro.Sale.DBContext.Interfaces;
using Dapper;
using Patika.Framework.Domain.Services;
using Patika.Framework.Shared.Extensions;
using Patika.Framework.Shared.Services;
using System.Data;

namespace CQRSMicro.Sale.DBContext.Services
{
    public class ProductQueryRepository : GenericDapperQueryRepository<Entities.Product, Guid>, IProductQueryRepository
    {
        public ProductQueryRepository(string connectionString, IServiceProvider serviceProvider) : base(connectionString, serviceProvider)
        {
        }
        //protected override PagedResult<Entities.Product> GetPagedResultWithIncludes(IDbConnection dbConnection) => dbConnection.GetPagedResult<Entities.Product>(
        //                                                                                                       SqlQueryBuilderGenerator
        //                                                                                                       .GenerateQueryBuilder<Entities.Product>()
        //                                                                                                       .PaginateQuery(null, 10)
        //                                                                                                       .ToString()
        //                                                                                                   ).GetAwaiter().GetResult();

        //protected override IQueryable<Entities.Product> GetQueryWithIncludes(IDbConnection dbConnection) => dbConnection.Query<Entities.Product>(SqlQueryBuilderGenerator.GenerateQueryBuilder<Entities.Product>().ToString()).AsQueryable();

    }
}