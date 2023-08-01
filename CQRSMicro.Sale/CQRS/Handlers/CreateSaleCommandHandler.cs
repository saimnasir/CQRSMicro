using CQRSMicro.Domain.Consts;
using CQRSMicro.Domain.Models;
using CQRSMicro.Sale.CQRS.Commands.Request;
using CQRSMicro.Sale.CQRS.Commands.Response;
using CQRSMicro.Sale.DBContext.Interfaces;
using MediatR;
using Patika.Framework.Shared.Services;
using Patika.Framework.Utilities.Queue.Interfaces;
using Patika.Framework.Utilities.Queue.Models.DTO;

namespace CQRSMicro.Sale.CQRS.Handlers
{
    public class CreateSaleCommandHandler : CoreService, IRequestHandler<CreateSaleCommandRequest, CreateSaleCommandResponse>
    {
        ISaleCUDRepository SaleCUDRepository { get; }
        IProductQueryRepository ProductQueryRepository { get; }
        //  IProductDAOCUDRepository ProductDAOCUDRepository { get; }

        IProducerService<ProductSoldModel> ProductSoldQueueService { get; }
        public CreateSaleCommandHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            SaleCUDRepository = GetService<ISaleCUDRepository>();
            //  ProductDAOCUDRepository = GetService<IProductDAOCUDRepository>();
            ProductSoldQueueService = GetService<IProducerService<ProductSoldModel>>();
            ProductQueryRepository = GetService<IProductQueryRepository>();
        }
        public async Task<CreateSaleCommandResponse> Handle(CreateSaleCommandRequest request, CancellationToken cancellationToken)
        {
            var id = Guid.NewGuid();
            var products = (await ProductQueryRepository.WhereAsync(x => request.Products.Select(s => s.Id).Contains(x.Id))).Queryable.ToList();
            var product = await SaleCUDRepository.InsertOneAsync(new()
            {
                Id = id,
                CreateTime = DateTime.Now,
                TotalPrice = products.Sum(s => s.Price),
                Products = products.Select(p => new DBContext.Entities.SaleProduct
                {
                    Id = Guid.NewGuid(),
                    Price = p.Price,
                    ProductId = p.Id,
                    Quantity = request.Products.FirstOrDefault(s => s.Id == p.Id)?.Quantity ?? 0
                }).ToList()
            });
            foreach (var p in request.Products)
            {
                await ProductSoldQueueService.PublishAsync(new QueueMessageDTO<ProductSoldModel>
                {
                    Message = new ProductSoldModel
                    {
                        Id = p.Id,
                        QuantitySold = p.Quantity,
                    },
                    QueueName = QueueConsts.ProductSold,
                    LogId = request.LogId,
                });
            }
            return new CreateSaleCommandResponse
            {
                IsSuccess = true,
                ProductId = id
            };
        }
    }
}
