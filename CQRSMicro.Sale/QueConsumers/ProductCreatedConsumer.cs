using CQRSMicro.Domain.Consts;
using CQRSMicro.Sale.DBContext.Interfaces;
using DotNetCore.CAP;
using Patika.Framework.Shared.Extensions;
using Patika.Framework.Shared.Interfaces;
using Patika.Framework.Shared.Services;
using Patika.Framework.Utilities.Queue.Models.DTO;
using Patika.Framework.Utilities.Queue.Services;

namespace CQRSMicro.Sale.QueConsumers
{
    public class ProductCreatedConsumer : ConsumerService<string>
    {
        ILogWriter LogWriter { get; }
        IProductCUDRepository ProductCUDRepository { get; }

        protected HttpClientService HttpClientService { get; set; }
        public ProductCreatedConsumer(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            LogWriter = GetService<ILogWriter>();
            ProductCUDRepository = GetService<IProductCUDRepository>();
            HttpClientService = new HttpClientService("https://localhost:7001", ServiceProvider);
        }

        [CapSubscribe(nameof(QueueConsts.ProductCreated))]
        public override async Task ConsumeAsync(QueueMessageDTO<string> input)
        {
            try
            {
                var product = await HttpClientService.HttpGetAs<DBContext.Entities.Product>($"/Product/id?Id={input.Message}") ?? throw new Exception("ProductNotFound");

                await ProductCUDRepository.InsertOneAsync(new()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    CreateTime = product.CreateTime
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
