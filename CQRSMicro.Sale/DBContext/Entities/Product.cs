using Patika.Framework.Shared.Entities;

namespace CQRSMicro.Sale.DBContext.Entities
{
    public class Product : Entity
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
