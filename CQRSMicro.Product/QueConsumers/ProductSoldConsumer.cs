using CQRSMicro.Domain.Consts;
using CQRSMicro.Domain.Models;
using CQRSMicro.Product.DBContext.Interfaces;
using DotNetCore.CAP;
using Patika.Framework.Shared.Extensions;
using Patika.Framework.Shared.Interfaces;
using Patika.Framework.Utilities.Queue.Models.DTO;
using Patika.Framework.Utilities.Queue.Services;

namespace CQRSMicro.Product.QueConsumers
{
    public class ProductSoldConsumer : ConsumerService<ProductSoldModel>
    {
        ILogWriter LogWriter { get; }
        IProductQueryRepository ProductQueryRepository { get; }
        IProductCUDRepository ProductCUDRepository { get; }
         
        public ProductSoldConsumer(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            LogWriter = GetService<ILogWriter>();
            ProductQueryRepository = GetService<IProductQueryRepository>();
            ProductCUDRepository = GetService<IProductCUDRepository>(); 
        }

        [CapSubscribe(nameof(QueueConsts.ProductSold))]
        public override async Task ConsumeAsync(QueueMessageDTO<ProductSoldModel> input)
        {
            try
            {
                if(input.Message is null)
                {
                    throw new ArgumentNullException(nameof(input.Message));
                }
                var product = await ProductQueryRepository.GetByIdAsync(input.Message.Id)?? throw new Exception("ProductNotFound");
                product.Quantity -= input.Message.QuantitySold;
                await ProductCUDRepository.UpdateOneAsync(product);
                await LogWriter.AddCodeMileStoneLogAsync(input, "Message consumed", GetType(), output: input.Message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
