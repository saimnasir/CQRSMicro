using CQRSMicro.Product.CQRS.Queries.Request;
using CQRSMicro.Product.CQRS.Queries.Response;
using CQRSMicro.Product.DBContext.Interfaces;
using MediatR;
using Patika.Framework.Shared.Services;

namespace CQRSMicro.Product.CQRS.Handlers
{
    public class GetByIdProductQueryHandler : CoreService, IRequestHandler<GetByIdProductQueryRequest, GetByIdProductQueryResponse>
    {
        IProductQueryRepository ProductQueryRepository { get; }
        public GetByIdProductQueryHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            ProductQueryRepository = GetService<IProductQueryRepository>();
        }

        public async Task<GetByIdProductQueryResponse> Handle(GetByIdProductQueryRequest request, CancellationToken cancellationToken)
        {
            var product = await ProductQueryRepository.GetByIdAsync(request.Id) ?? throw new Exception("ProductNotFound");

            return new GetByIdProductQueryResponse
            {
                CreateTime = product.CreateTime,
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity
            };
        }
    }
}
