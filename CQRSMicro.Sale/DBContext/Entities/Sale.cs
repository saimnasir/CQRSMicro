using Patika.Framework.Shared.Entities;

namespace CQRSMicro.Sale.DBContext.Entities
{
    public class Sale : Entity
    {       
        public decimal TotalPrice { get; set; }
        public DateTime CreateTime { get; set; }
        public virtual Customer? Customer { get; set; }
        public Guid CustomerId { get; set; }
        public virtual ICollection<SaleProduct> Products { get; set; } = new List<SaleProduct>();
    }
}
