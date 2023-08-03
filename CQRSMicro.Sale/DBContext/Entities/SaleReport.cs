using Patika.Framework.Shared.Entities;

namespace CQRSMicro.Sale.DBContext.Entities
{
    public class SaleReport : Entity
    {
        public Guid SaleId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal SoldPrice { get; set; }
        public int ProductQuantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
