using CQRSMicro.Domain.Models.Enums;
using Patika.Framework.Shared.Entities;

namespace CQRSMicro.CustomerApp.DBContext.Entities
{
    public class Customer : Entity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Age { get; set; } 
        public GenderEnum Gender { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
