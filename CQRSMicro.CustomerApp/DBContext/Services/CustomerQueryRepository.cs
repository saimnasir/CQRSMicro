using CQRSMicro.CustomerApp.DBContext.Interfaces;
using Dapper;
using Patika.Framework.Domain.Services;
using Patika.Framework.Shared.Extensions;
using Patika.Framework.Shared.Services;
using System.Data;

namespace CQRSMicro.CustomerApp.DBContext.Services
{
    public class CustomerQueryRepository : GenericDapperRepository<Entities.Customer, Guid>, ICustomerQueryRepository
    {
        public CustomerQueryRepository(string connectionString, IServiceProvider serviceProvider) : base(connectionString, serviceProvider)
        {
        }


        //protected override PagedResult<Entities.Customer> GetPagedResultWithIncludes(IDbConnection dbConnection) => dbConnection.GetPagedResult<Entities.Customer>(
        //                                                                                                        SqlQueryBuilderGenerator
        //                                                                                                        .GenerateQueryBuilder<Entities.Customer>()
        //                                                                                                        .PaginateQuery(null, 10)
        //                                                                                                        .ToString()
        //                                                                                                    ).GetAwaiter().GetResult();

        //protected override IQueryable<Entities.Customer> GetQueryWithIncludes(IDbConnection dbConnection) => dbConnection.Query<Entities.Customer>(SqlQueryBuilderGenerator.GenerateQueryBuilder<Entities.Customer>().ToString()).AsQueryable();

    }
}