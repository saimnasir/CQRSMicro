using CQRSMicro.CustomerApp.CQRS.Commands.Request;
using CQRSMicro.CustomerApp.CQRS.Commands.Response;
using CQRSMicro.CustomerApp.DBContext.Interfaces;
using CQRSMicro.Domain.Consts;
using MediatR;
using Patika.Framework.Shared.Services;
using Patika.Framework.Utilities.Queue.Interfaces;
using Patika.Framework.Utilities.Queue.Models.DTO;

namespace CQRSMicro.CustomerApp.CQRS.Handlers
{
    public class CreateCustomerCommandHandler : CoreService, IRequestHandler<CreateCustomerCommandRequest, CreateCustomerCommandResponse>
    {
        ICustomerCUDRepository CustomerCUDRepository { get; }
        //  ICustomerDAOCUDRepository CustomerDAOCUDRepository { get; }

        IProducerService<Guid> CustomerCreatedQueueService { get; }
        public CreateCustomerCommandHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            CustomerCUDRepository = GetService<ICustomerCUDRepository>();
            //  CustomerDAOCUDRepository = GetService<ICustomerDAOCUDRepository>();
            CustomerCreatedQueueService = GetService<IProducerService<Guid>>();
        }
        public async Task<CreateCustomerCommandResponse> Handle(CreateCustomerCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var id = Guid.NewGuid();
                var customer = await CustomerCUDRepository.InsertOneAsync(new()
                {
                    Id = id,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Age = request.Age,
                    Gender = request.Gender,
                    CreateTime = DateTime.Now
                });
                await CustomerCreatedQueueService.PublishAsync(new QueueMessageDTO<Guid>
                {
                    Message = customer.Id,
                    QueueName = QueueConsts.CustomerCreated,
                    LogId = request.LogId,
                });
                return new CreateCustomerCommandResponse
                {
                    IsSuccess = true,
                    CustomerId = id
                };
            }
            catch(Exception ex)
            {
                throw;
            }
          
        }
    }
}
