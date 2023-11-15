namespace CQRSMicro.Product.Fuzzy.Models
{
    public class Campaign
    {
        public int BrandId { get; set; }  
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty; 
        public string CampaignType { get; set; } = string.Empty;
        public Brand? Brand { get; set; } 
    }
}
