using CQRSMicro.Domain.Models.Enums;

namespace CQRSMicro.CustomerApp.CQRS.Queries.Response
{
    public class GetByIdCustomerQueryResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Age { get; set; }
        public GenderEnum Gender { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
