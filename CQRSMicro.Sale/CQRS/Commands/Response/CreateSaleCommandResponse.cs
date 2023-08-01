namespace CQRSMicro.Sale.CQRS.Commands.Response
{
    public class CreateSaleCommandResponse
    {
        public bool IsSuccess { get; set; }
        public Guid ProductId { get; set; }
    }
}
