using CQRSMicro.Sale.CQRS.Queries.Response;
using MediatR;
using System.Linq.Dynamic.Core;

namespace CQRSMicro.Sale.CQRS.Queries.Request
{
    public class GetAllSaleQueryRequest : IRequest<PagedResult<GetAllSaleQueryResponse>>
    {
    }
}
