using Patika.Framework.Shared.Entities;

namespace CQRSMicro.Sale.DBContext.Entities
{
    public class SaleProduct : Entity
    {
        public Guid SaleId { get; set; }
        public virtual Sale Sale {get;set;}= new();
        public Guid ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
