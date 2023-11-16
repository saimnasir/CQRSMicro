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
        public List<int> MatchedPositions { get; set; }
        public string DataType { get; set; }
    }
    public class SearchResultModelV2
    {
        public int OverallScore { get; set; }
        public Campaign Campaign { get; set; } = new();
        public SearchResult BrandSearchResult { get; set; } = new();
        public SearchResult CampaignSearchResult { get; set; } = new();
    }
    public class SearchSuggestionModel
    {
        public string Keyword { get; set; } = string.Empty;
        public SearchResult SearchResult { get; set; } = new();
    }
    public class SearchResult
    {
        public int Score { get; set; }
        public string Highlighted { get; set; } = string.Empty;
        public string SearchString { get; set; } = string.Empty;
        public List<int> MatchedPositions { get; set; }
    }
}
