using CQRSMicro.Product.CQRS.Queries.Request;
using CQRSMicro.Product.CQRS.Queries.Response;
using CQRSMicro.Product.DBContext.Interfaces;
using MediatR;
using Patika.Framework.Shared.Extensions;
using Patika.Framework.Shared.Services;
using System.Linq.Dynamic.Core;

namespace CQRSMicro.Product.CQRS.Handlers
{
    public class GetAllProductQueryHandler : CoreService, IRequestHandler<GetAllProductQueryRequest, PagedResult<GetAllProductQueryResponse>>
    {
        IProductQueryRepository ProductQueryRepository { get; }
        public GetAllProductQueryHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            ProductQueryRepository = GetService<IProductQueryRepository>();
        }

        public async Task<PagedResult<GetAllProductQueryResponse>> Handle(GetAllProductQueryRequest getAllProductQueryRequest, CancellationToken cancellationToken)
        {
            try
            {
                var result = await ProductQueryRepository.GetAllAsync();
                var list = result.Queryable.Select(s => new GetAllProductQueryResponse
                {
                    CreateTime = s.CreateTime,
                    Id = s.Id,
                    Name = s.Name,
                    Price = s.Price,
                    Quantity = s.Quantity
                });
                return await list.PaginateAsync(new Patika.Framework.Shared.Entities.Pagination());
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
