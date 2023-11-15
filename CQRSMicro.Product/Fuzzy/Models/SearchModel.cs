namespace CQRSMicro.Product.Fuzzy.Models
{
    public class SearchModel
    {
        public int BrandId { get; set; }
        public DataType DataType { get; set; }
        public string FuzzySearchString { get; set; } = string.Empty;
        public string QueryText { get; set; } = string.Empty; 
    }
    public enum DataType
    {
        Brand,
        Campaign
    }
    public class SearchResultModel
    {  
        public int Score { get; set; }
        public int BrandId { get; set; }
        public Campaign Campaign { get; set; } = new();
        public DataType DataType { get; set; }
    }
}
