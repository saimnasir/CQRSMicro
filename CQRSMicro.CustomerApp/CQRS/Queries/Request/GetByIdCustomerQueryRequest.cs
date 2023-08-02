using CQRSMicro.CustomerApp.CQRS.Queries.Response;
using MediatR;

namespace CQRSMicro.CustomerApp.CQRS.Queries.Request
{
    public class GetByIdCustomerQueryRequest : IRequest<GetByIdCustomerQueryResponse>
    {
        public Guid Id { get; set; }
    }
}
