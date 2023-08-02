using CQRSMicro.CustomerApp.CQRS.Queries.Request;
using CQRSMicro.CustomerApp.CQRS.Queries.Response;
using CQRSMicro.CustomerApp.DBContext.Interfaces;
using MediatR;
using Patika.Framework.Shared.Services;

namespace CQRSMicro.CustomerApp.CQRS.Handlers
{
    public class GetByIdCustomerQueryHandler : CoreService, IRequestHandler<GetByIdCustomerQueryRequest, GetByIdCustomerQueryResponse>
    {
        ICustomerQueryRepository CustomerQueryRepository { get; }
        public GetByIdCustomerQueryHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            CustomerQueryRepository = GetService<ICustomerQueryRepository>();
        }

        public async Task<GetByIdCustomerQueryResponse> Handle(GetByIdCustomerQueryRequest request, CancellationToken cancellationToken)
        {
            var customer = await CustomerQueryRepository.GetByIdAsync(request.Id) ?? throw new Exception("CustomerNotFound");

            return new GetByIdCustomerQueryResponse
            {
                CreateTime = customer.CreateTime,
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Gender = customer.Gender,
                Age = customer.Age,
            };
        }
    }
}
