using CQRSMicro.CustomerApp.CQRS.Commands.Response;
using CQRSMicro.Domain.Models.Enums;
using MediatR;
using Patika.Framework.Shared.DTO;

namespace CQRSMicro.CustomerApp.CQRS.Commands.Request
{
    public class CreateCustomerCommandRequest : DTO, IRequest<CreateCustomerCommandResponse>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Age { get; set; }
        public GenderEnum Gender { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
