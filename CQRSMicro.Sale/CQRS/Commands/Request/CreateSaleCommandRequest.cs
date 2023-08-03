using CQRSMicro.Sale.CQRS.Commands.Response;
using MediatR;
using Patika.Framework.Shared.DTO;

namespace CQRSMicro.Sale.CQRS.Commands.Request
{
    public class CreateSaleCommandRequest : DTO, IRequest<CreateSaleCommandResponse>
    {
        public Guid CustomerId { get; set; }
        public virtual ICollection<SaleProductDTO> Products { get; set; } = new List<SaleProductDTO>();
    }
    public class SaleProductDTO
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
    }
}
