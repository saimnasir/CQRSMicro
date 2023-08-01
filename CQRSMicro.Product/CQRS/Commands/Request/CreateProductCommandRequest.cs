using CQRSMicro.Product.CQRS.Commands.Response;
using MediatR;
using Patika.Framework.Shared.DTO;

namespace CQRSMicro.Product.CQRS.Commands.Request
{
    public class CreateProductCommandRequest : DTO, IRequest<CreateProductCommandResponse>
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
