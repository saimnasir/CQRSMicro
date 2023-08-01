using Patika.Framework.Shared.Entities;

namespace CQRSMicro.Product.DBContext.Entities
{
    public class Product : Entity
    {
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
