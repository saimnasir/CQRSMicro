using CQRSMicro.Domain.Consts;
using CQRSMicro.Product.CQRS.Commands.Request;
using CQRSMicro.Product.CQRS.Commands.Response;
using CQRSMicro.Product.DBContext.Interfaces;
using MediatR;
using Patika.Framework.Shared.Services;
using Patika.Framework.Utilities.Queue.Interfaces;
using Patika.Framework.Utilities.Queue.Models.DTO;

namespace CQRSMicro.Product.CQRS.Handlers
{
    public class CreateProductCommandHandler : CoreService, IRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse>
    {
        IProductCUDRepository ProductCUDRepository { get; }
      //  IProductDAOCUDRepository ProductDAOCUDRepository { get; }

        IProducerService<Guid> ProductCreatedQueueService { get; }
        public CreateProductCommandHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            ProductCUDRepository = GetService<IProductCUDRepository>();
          //  ProductDAOCUDRepository = GetService<IProductDAOCUDRepository>();
            ProductCreatedQueueService = GetService<IProducerService<Guid>>();
        }
        public async Task<CreateProductCommandResponse> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
        {
            var id = Guid.NewGuid();
            var product = await ProductCUDRepository.InsertOneAsync(new()
            {
                Id = id,
                Name = request.Name,
                Price = request.Price,
                Quantity = request.Quantity,
                CreateTime = DateTime.Now
            });
            await ProductCreatedQueueService.PublishAsync(new QueueMessageDTO<Guid>
            {
                Message = product.Id,
                QueueName = QueueConsts.ProductCreated,
                LogId = request.LogId,
            });
            return new CreateProductCommandResponse
            {
                IsSuccess = true,
                ProductId = id
            };
        }
    }
}
