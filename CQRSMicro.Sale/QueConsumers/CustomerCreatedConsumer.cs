using CQRSMicro.Domain.Consts;
using CQRSMicro.Sale.DBContext.Interfaces;
using CQRSMicro.Sale.DTOs;
using DotNetCore.CAP;
using Patika.Framework.Shared.Extensions;
using Patika.Framework.Shared.Interfaces;
using Patika.Framework.Shared.Services;
using Patika.Framework.Utilities.Queue.Models.DTO;
using Patika.Framework.Utilities.Queue.Services;

namespace CQRSMicro.Sale.QueConsumers
{
    public class CustomerCreatedConsumer : ConsumerService<string>
    {
        ILogWriter LogWriter { get; }
        ICustomerCUDRepository CustomerCUDRepository { get; }

        protected HttpClientService HttpClientService { get; set; }
        public CustomerCreatedConsumer(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            LogWriter = GetService<ILogWriter>();
            CustomerCUDRepository = GetService<ICustomerCUDRepository>();
            HttpClientService = new HttpClientService("https://localhost:7003", ServiceProvider);
        }

        [CapSubscribe(nameof(QueueConsts.CustomerCreated))]
        public override async Task ConsumeAsync(QueueMessageDTO<string> input)
        {
            try
            {
                var customer = await HttpClientService.HttpGetAs<GetByIdCustomerDTO>($"/Customer/id?Id={input.Message}") ?? throw new Exception("CustomerNotFound");

                await CustomerCUDRepository.InsertOneAsync(new()
                {
                    Id = customer.Id,
                    FullName = $"{customer.FirstName} {customer.LastName}",
                    Age = customer.Age,
                    Gender = customer.Gender
                });
                await LogWriter.AddCodeMileStoneLogAsync(input, "Message consumed", GetType(), output: input.Message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
