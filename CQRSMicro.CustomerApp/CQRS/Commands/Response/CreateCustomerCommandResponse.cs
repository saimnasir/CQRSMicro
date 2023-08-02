namespace CQRSMicro.CustomerApp.CQRS.Commands.Response
{
    public class CreateCustomerCommandResponse
    {
        public bool IsSuccess { get; set; }
        public Guid CustomerId { get; set; }
    }
}
