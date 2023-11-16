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
        public int BrandScore { get; set; }
        public int Score { get; set; }
        public int BrandId { get; set; }
        public Campaign Campaign { get; set; } = new();
        public string Highlighted { get; set; } = string.Empty;
        public List<int> MatchedPositions { get;   set; }
        public string DataType { get;  set; }
    }
    public class SearchResultModelV2
    {
        public int BrandScore { get; set; }
        public int CampaignScore { get; set; } 
        public int OverallScore { get; set; } 
        public Campaign Campaign { get; set; } = new();
        public string HighlightedCampaign { get; set; } = string.Empty;
        public string HighlightedBrand { get; set; } = string.Empty;
        public List<int> MatchedPositionsBrand { get; set; }
        public List<int> MatchedPositionsCampaign { get; set; }
    }
    public class SearchSuggestionModel
    {
        public int  Score { get; set; }
        public string Keyword { get; set; } = string.Empty;
    } 
}
