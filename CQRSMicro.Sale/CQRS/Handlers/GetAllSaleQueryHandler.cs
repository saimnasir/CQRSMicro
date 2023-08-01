//using CQRSMicro.Product.CQRS.Queries.Request;
//using CQRSMicro.Product.CQRS.Queries.Response;
//using MediatR;
//using Patika.Framework.Shared.Extensions;
//using Patika.Framework.Shared.Services;
//using System.Linq.Dynamic.Core;

//namespace CQRSMicro.Product.CQRS.Handlers
//{
//    public class GetAllSaleQueryHandler : CoreService, IRequestHandler<GetAllProductQueryRequest, PagedResult<GetAllProductQueryResponse>>
//    {
//        IProductDAOQueryRepository ProductDAOQueryRepository { get; }
//        public GetAllProductQueryHandler(IServiceProvider serviceProvider) : base(serviceProvider)
//        {
//            ProductDAOQueryRepository = GetService<IProductDAOQueryRepository>();
//        }

//        public async Task<PagedResult<GetAllProductQueryResponse>> Handle(GetAllProductQueryRequest getAllProductQueryRequest, CancellationToken cancellationToken)
//        {
//            var result = await ProductDAOQueryRepository.GetAllAsync();
//            var list = result.Queryable.Select(s => new GetAllProductQueryResponse
//            {
//                CreateTime = s.CreateTime,
//                Id = s.Id,
//                Name = s.Name,
//                Price = s.Price,
//                Quantity = s.Quantity
//            });
//            return await list.PaginateAsync(new Patika.Framework.Shared.Entities.Pagination());
//        }
//    }
//}
