namespace CQRSMicro.Sale.CQRS.Queries.Response
{
    public class GetByIdSaleQueryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
