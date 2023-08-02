using CQRSMicro.Domain.Models.Enums;
using Patika.Framework.Shared.Entities;

namespace CQRSMicro.Sale.DBContext.Entities
{
    public class Customer : Entity
    {
        public string FullName { get; set; } = string.Empty; 
        public int Age { get; set; }
        public GenderEnum Gender { get; set; } 
    }
}
