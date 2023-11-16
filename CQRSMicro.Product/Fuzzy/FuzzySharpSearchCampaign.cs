using CQRSMicro.Product.Fuzzy.Models;
using FuzzySharp;

namespace CQRSMicro.Product.Fuzzy
{
    public class FuzzySharpSearchCampaign
    {
        public IEnumerable<SearchSuggestionModel> Suggestions(string query)
        {
            query = query.ToLower();
            var campaigns = Data.Campaigns.ToList();
            campaigns.ForEach(campaing =>
            {
                campaing.Brand = Data.Brands.FirstOrDefault(b => b.Id == campaing.BrandId) ?? new Brand();
            });

            var campaignSearchResult = SearchSuggestions(campaigns, query);
            var orderedResults = campaignSearchResult.OrderByDescending(result => result.SearchResult.Score);

            var result = orderedResults.Where(item => item.SearchResult.Score > 20).Take(10).ToList();

            return result;
        }

        public IEnumerable<SearchResultModelV2> SearchAllCampaignsV2(string query)
        {
            query = query.ToLower();
            var campaigns = Data.Campaigns.ToList();
            campaigns.ForEach(campaing =>
            {
                campaing.Brand = Data.Brands.FirstOrDefault(b => b.Id == campaing.BrandId) ?? new Brand();
            });
            var campaignSearchResult = SearchFromCampaignsV2(campaigns, query);

            var orderedResults = campaignSearchResult.OrderByDescending(result => result.OverallScore);

            var result = orderedResults.Where(item => item.OverallScore > 20).Take(10).ToList();

            return result;
        }

        public IEnumerable<SearchResultModel> SearchAllCampaigns(string query)
        {
            query = query.ToLower();
            IEnumerable<SearchResultModel> campaigns = SearchFromCampaigns(Data.Campaigns, query);

            IEnumerable<SearchResultModel> campaignsOverBrands = SearchFromBrands(query);

            var concatedResults = campaigns.Concat(campaignsOverBrands).ToList();
            var orderedResults = concatedResults.GroupBy(result => result.Campaign.Title)
                .Select(group => group.OrderByDescending(result => result.Score).First())
                .OrderByDescending(result => result.Score);

            var result = orderedResults.Where(brand => brand.Score > 20).Take(10).ToList();
            result.ForEach(item => item.Campaign.Brand = Data.Brands.FirstOrDefault(b => b.Id == item.BrandId));

            return result;
        }

        private static IEnumerable<SearchResultModel> SearchFromBrands(string query)
        {
            var searchs = Data.Brands
                .Select(brand => new
                {
                    SearchString = BrandFuzzySearchString(brand),
                    Brand = brand,
                    BrandScore = Fuzz.WeightedRatio(query, BrandFuzzySearchString(brand), FuzzySharp.PreProcess.PreprocessMode.Full),
                    MatchedPositions = GetMatchedPositions(query, BrandFuzzySearchString(brand)),
                    Campaigns = Data.Campaigns.Where(campaign => campaign.BrandId == brand.Id),
                });

            var highlights = searchs
                .Select(brand => new
                {
                    brand.Brand,
                    brand.BrandScore,
                    brand.MatchedPositions,
                    brand.Campaigns,
                    Highlighted = HighlightMatchedPart(brand.SearchString, brand.MatchedPositions),
                });

            var brands = highlights
                .SelectMany(brand => brand.Campaigns.Select(campaign => new SearchResultModel
                {
                    Campaign = campaign,
                    BrandId = brand.Brand.Id,
                    BrandScore = brand.BrandScore,
                    DataType = typeof(Brand).Name,
                    Highlighted = brand.Highlighted,
                    MatchedPositions = brand.MatchedPositions
                }))
                .OrderByDescending(result => result.BrandScore)
                .Where(brand => brand.BrandScore > 50);

            var campaignList = brands.SelectMany(brand => Data.Campaigns.Where(c => c.BrandId == brand.BrandId)).ToList();

            IEnumerable<SearchResultModel> campaigns = SearchFromCampaigns(campaignList, query);
            return campaigns;
        }

        private static IEnumerable<SearchResultModelV2> SearchFromCampaignsV2(List<Campaign> campaignList, string query)
        {
            var searches = campaignList.Select(campaign =>
            {
                return new
                {
                    CampaignSearchString = CampaignFuzzySearchString(campaign),
                    BrandSearchString = BrandFuzzySearchString(campaign.Brand),
                    Campaign = campaign,
                    CampaignScore = Fuzz.WeightedRatio(query, CampaignFuzzySearchString(campaign), FuzzySharp.PreProcess.PreprocessMode.Full),
                    BrandScore = Fuzz.WeightedRatio(query, BrandFuzzySearchString(campaign.Brand), FuzzySharp.PreProcess.PreprocessMode.Full),
                    MatchedPositionsCampaign = GetMatchedPositions(query, CampaignFuzzySearchString(campaign)),
                    MatchedPositionsBrand = GetMatchedPositions(query, BrandFuzzySearchString(campaign.Brand))
                };
            });
            var campaigns = searches.Select(campaign =>
            {
                return new SearchResultModelV2
                {
                    Campaign = campaign.Campaign,
                    OverallScore = (campaign.CampaignScore + campaign.BrandScore) / 2,
                    BrandSearchResult = new SearchResult
                    {
                        MatchedPositions = campaign.MatchedPositionsBrand,
                        Highlighted = HighlightMatchedPart(campaign.BrandSearchString, campaign.MatchedPositionsBrand),
                        Score = campaign.BrandScore,
                    },
                    CampaignSearchResult = new SearchResult
                    {
                        MatchedPositions = campaign.MatchedPositionsCampaign,
                        Highlighted = HighlightMatchedPart(campaign.CampaignSearchString, campaign.MatchedPositionsCampaign),
                        Score = campaign.CampaignScore,
                    }
                };
            })
                .OrderByDescending(campaign => campaign.OverallScore);
            return campaigns;
        }
        private static IEnumerable<SearchSuggestionModel> SearchSuggestions(List<Campaign> campaignList, string query)
        {
            var searches = campaignList.Select(campaign =>
            {
                return new
                {
                    Campaign = campaign,
                    CampaignScore = Fuzz.WeightedRatio(query, CampaignFuzzySearchString(campaign), FuzzySharp.PreProcess.PreprocessMode.Full),
                    BrandScore = Fuzz.WeightedRatio(query, BrandFuzzySearchString(campaign.Brand), FuzzySharp.PreProcess.PreprocessMode.Full),
                    HighlightedCampaign = HighlightMatchedPart(CampaignFuzzySearchString(campaign), GetMatchedPositions(query, CampaignFuzzySearchString(campaign))),
                    HighlightedBrand = HighlightMatchedPart(BrandFuzzySearchString(campaign.Brand), GetMatchedPositions(query, BrandFuzzySearchString(campaign.Brand))),
                    CampaignSearchString = CampaignFuzzySearchString(campaign),
                    BrandSearchString = BrandFuzzySearchString(campaign.Brand),
                    //MatchedPositionsCampaign = GetMatchedPositions(query, CampaignFuzzySearchString(campaign)),
                    //MatchedPositionsBrand = GetMatchedPositions(query, BrandFuzzySearchString(campaign.Brand))
                };
            });
            var campaigns = searches.Select(item =>
            {
                return new SearchSuggestionModel
                {
                    Keyword = item.BrandScore >= item.CampaignScore ? item.Campaign.Brand.Name : item.Campaign.Title,
                    SearchResult = new SearchResult
                    {
                        Score = Math.Max(item.CampaignScore, item.BrandScore),
                        Highlighted = item.BrandScore >= item.CampaignScore ? item.HighlightedBrand : item.HighlightedCampaign,
                        SearchString = item.BrandScore >= item.CampaignScore ? item.BrandSearchString : item.CampaignSearchString
                    }
                };
            })
                .OrderByDescending(campaign => campaign.SearchResult.Score);
            return campaigns;
        }
        private static IEnumerable<SearchResultModel> SearchFromCampaigns(List<Campaign> campaignList, string query)
        {
            var searches = campaignList.Select(campaign =>
            {
                return new
                {
                    SearchString = CampaignFuzzySearchString(campaign),
                    Campaign = campaign,
                    Score = Fuzz.WeightedRatio(query, CampaignFuzzySearchString(campaign), FuzzySharp.PreProcess.PreprocessMode.Full),
                    MatchedPositions = GetMatchedPositions(query, CampaignFuzzySearchString(campaign)),
                    DataType = DataType.Campaign
                };
            });
            var campaigns = searches.Select(campaign =>
            {
                return new SearchResultModel
                {
                    Campaign = campaign.Campaign,
                    Score = campaign.Score,
                    DataType = typeof(Campaign).Name,
                    MatchedPositions = campaign.MatchedPositions,
                    Highlighted = HighlightMatchedPart(campaign.SearchString, campaign.MatchedPositions),
                };
            })
                .OrderByDescending(campaign => campaign.Score);
            return campaigns;
        }

        private static string CampaignFuzzySearchString(Campaign campaign)
        {
            return $"{campaign.Title} {campaign.Description} {campaign.Content} {campaign.CampaignType}";
        }
        private static string BrandFuzzySearchString(Brand brand)
        {
            return $"{brand.Name} {brand.Sector} {brand.About} {brand.BrandType}";
        }
        private static List<int> GetMatchedPositions(string query, string target)
        {
            var matchedPositions = new List<int>();

            int queryIndex = 0;
            int targetIndex = 0;

            while (queryIndex < query.Length && targetIndex < target.Length)
            {
                if (query[queryIndex].ToString().Equals(target[targetIndex].ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    matchedPositions.Add(targetIndex);
                    queryIndex++;
                }

                targetIndex++;
            }

            return matchedPositions;
        }
        private static string HighlightMatchedPart(string original, List<int> matchedPositions)
        {
            if (!matchedPositions.Any())
                return string.Empty;

            var highlightedPart = string.Join("", original
                .Select((c, index) => matchedPositions.Contains(index) ? $"<b>{c}</b>" : c.ToString()));

            return $"<span>{highlightedPart}</span>";
        }
    }
}
