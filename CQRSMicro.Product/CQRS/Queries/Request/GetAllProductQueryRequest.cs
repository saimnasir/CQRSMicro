using CQRSMicro.Product.CQRS.Queries.Response;
using MediatR;
using System.Linq.Dynamic.Core;

namespace CQRSMicro.Product.CQRS.Queries.Request
{
    public class GetAllProductQueryRequest : IRequest<PagedResult<GetAllProductQueryResponse>>
    {
    }
}
