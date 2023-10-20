namespace CQRSMicro.Product.Fuzzy.Models
{
    public class Campaign
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string CampaignType { get; set; } = string.Empty;
    }
}
