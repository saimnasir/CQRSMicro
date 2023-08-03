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
        IProducerService<Guid> SaleCreatedQueueService { get; }
        public CreateSaleCommandHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            SaleCUDRepository = GetService<ISaleCUDRepository>();
            //  ProductDAOCUDRepository = GetService<IProductDAOCUDRepository>();
            ProductSoldQueueService = GetService<IProducerService<ProductSoldModel>>();
            SaleCreatedQueueService = GetService<IProducerService<Guid>>();
            ProductQueryRepository = GetService<IProductQueryRepository>();
        }
        public async Task<CreateSaleCommandResponse> Handle(CreateSaleCommandRequest request, CancellationToken cancellationToken)
        {
            var id = Guid.NewGuid();
            var products = (await ProductQueryRepository.WhereAsync(x => request.Products.Select(s => s.Id).Contains(x.Id))).Queryable.ToList();
            if (!products.Any())
            {
                throw new Exception("AttLeastOneProductsRequired");
            }
            var sale = new DBContext.Entities.Sale
            {
                Id = id,
                CreateTime = DateTime.Now,
                TotalPrice = products.Sum(p => p.Price * getQuantity(request,p)),
                CustomerId = request.CustomerId,
                Products = products.Select(p =>
                {
                    int quantity = getQuantity(request, p);
                    return new DBContext.Entities.SaleProduct
                    {
                        Id = Guid.NewGuid(),
                        Price = p.Price * quantity,
                        ProductId = p.Id,
                        Quantity = quantity
                    };
                }).ToList()
            };
            await SaleCUDRepository.InsertOneAsync(sale);
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
            await SaleCreatedQueueService.PublishAsync(new QueueMessageDTO<Guid>
            {
                Message = sale.Id,
                QueueName = QueueConsts.SaleCreated,
                LogId = request.LogId,
            });
            return new CreateSaleCommandResponse
            {
                IsSuccess = true,
                ProductId = id
            };

            static int getQuantity(CreateSaleCommandRequest request, DBContext.Entities.Product product)
            {
                return request.Products.FirstOrDefault(s => s.Id == product.Id)?.Quantity ?? 0;
            }
        }
    }
}
