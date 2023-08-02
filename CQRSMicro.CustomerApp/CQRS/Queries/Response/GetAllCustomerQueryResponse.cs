namespace CQRSMicro.CustomerApp.CQRS.Queries.Response
{
    public class GetAllCustomerQueryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
