using CQRSMicro.Domain.Models.Enums;

namespace CQRSMicro.Sale.DTOs
{
    public class GetByIdCustomerDTO
    {
        public Guid Id { get; set; } 
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Age { get; set; }
        public GenderEnum Gender { get; set; }
    }
}
