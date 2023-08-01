using CQRSMicro.Product.CQRS.Queries.Response;
using MediatR;

namespace CQRSMicro.Product.CQRS.Queries.Request
{
    public class GetByIdProductQueryRequest : IRequest<GetByIdProductQueryResponse>
    {
        public Guid Id { get; set; }
    }
}
