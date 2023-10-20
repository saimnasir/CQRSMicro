namespace CQRSMicro.Product.Fuzzy.Models
{
    public class Brand
    {
        public string Name { get; set; } = string.Empty;
        public string Sector { get; set; } = string.Empty;
        public string About { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string BrandType { get; set; } = string.Empty;
    }
}
