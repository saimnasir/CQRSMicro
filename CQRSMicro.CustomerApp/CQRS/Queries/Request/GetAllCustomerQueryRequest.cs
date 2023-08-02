using CQRSMicro.CustomerApp.CQRS.Queries.Response;
using MediatR;
using System.Linq.Dynamic.Core;

namespace CQRSMicro.CustomerApp.CQRS.Queries.Request
{
    public class GetAllCustomerQueryRequest : IRequest<PagedResult<GetAllCustomerQueryResponse>>
    {
    }
}
