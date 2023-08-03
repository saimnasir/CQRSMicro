using CQRSMicro.Domain.Consts;
using CQRSMicro.Sale.DBContext.Entities;
using CQRSMicro.Sale.DBContext.Interfaces;
using DotNetCore.CAP;
using Patika.Framework.Shared.Extensions;
using Patika.Framework.Shared.Interfaces;
using Patika.Framework.Utilities.Queue.Models.DTO;
using Patika.Framework.Utilities.Queue.Services;

namespace CQRSMicro.Sale.QueConsumers
{
    public class SaleCreatedConsumer : ConsumerService<Guid>
    {
        ILogWriter LogWriter { get; }
        ISaleQueryRepository SaleQueryRepository { get; }
        ISaleReportCUDRepository SaleReportCUDRepository { get; }
        IProductQueryRepository ProductQueryRepository { get; }

        public SaleCreatedConsumer(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            LogWriter = GetService<ILogWriter>();
            SaleQueryRepository = GetService<ISaleQueryRepository>();
            SaleReportCUDRepository = GetService<ISaleReportCUDRepository>();
            ProductQueryRepository = GetService<IProductQueryRepository>();
        }

        [CapSubscribe(nameof(QueueConsts.SaleCreated))]
        public override async Task ConsumeAsync(QueueMessageDTO<Guid> input)
        {
            var id = Guid.NewGuid();
            try
            {
                var sale = await SaleQueryRepository.GetByIdAsync(input.Message, includeChilds: true) ?? throw new Exception("SaleNotFound");
                var soldProducts = (await ProductQueryRepository.WhereAsync(p => sale.Products.Select(sp => sp.ProductId).Contains(p.Id))).Queryable.ToList(); ;
                var saleReports = sale.Products.Select(sp => new SaleReport
                {
                    Id = id,
                    CustomerId = sale.CustomerId,
                    ProductId = sp.ProductId,
                    ProductName = soldProducts.First(p => p.Id == sp.ProductId).Name,
                    ProductQuantity = sp.Quantity,
                    SaleId = sale.Id,
                    SoldPrice = sp.Price,
                    TotalPrice = sale.TotalPrice,
                }).ToList();
                await SaleReportCUDRepository.InsertManyAsync(saleReports);
                await LogWriter.AddCodeMileStoneLogAsync(input, "Message consumed", GetType(), output: input.Message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
