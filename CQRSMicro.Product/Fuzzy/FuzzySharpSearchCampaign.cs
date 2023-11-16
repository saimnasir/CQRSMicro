using CQRSMicro.Product.Fuzzy.Models;
using FuzzySharp;

namespace CQRSMicro.Product.Fuzzy
{
    public class FuzzySharpSearchCampaign
    {
        //public List<SearchResultModel> SearchCampaigns(string query)
        //{
        //    // var campaignStrings =Data.Campaigns.Select(c => $"{c.Title} {c.Description}").ToList();
        //    var campaignStrings = Data.Campaigns.Select(c => new SearchModel
        //    {
        //        QueryText = query,
        //        BrandId = c.BrandId,
        //        DataType = DataType.Campaign,
        //        FuzzySearchString = $" {c.BrandId} {c.Title} {c.Description} {c.Content} {c.CampaignType} {c.Content}",

        //    }).ToList();
        //    var results = ProcessSearchQuery(query, campaignStrings);
        //    return results.Select(x => x).Take(10).ToList(); // İlk 10 sonucu döndür
        //}

        //public List<SearchResultModel> SearchBrands(string query)
        //{
        //    var brandStrings = Data.Brands.Select(b => new SearchModel
        //    {
        //        QueryText = query,
        //        BrandId = b.Id,
        //        DataType = DataType.Campaign,
        //        FuzzySearchString = $"{b.Id} {b.Name} {b.Sector} {b.About} {b.BrandType}"
        //    }).ToList();
        //    var results = ProcessSearchQuery(query, brandStrings);
        //    return results.Select(x => x).Take(10).ToList(); // İlk 10 sonucu döndür
        //}

        //public List<SearchResultModel> ProcessSearchQuery(string query, List<SearchModel> items)
        //{
        //    var searchResults = new List<SearchResultModel>();
        //    foreach (var item in items)
        //    {
        //        var score = Fuzz.PartialRatio(query, item.FuzzySearchString);
        //        searchResults.Add(new SearchResultModel { FuzzySearchString = item.FuzzySearchString, Score = score });
        //    }
        //    // En yüksek skorları alarak sırala
        //    searchResults = searchResults.OrderByDescending(x => x.Score).ToList();
        //    return searchResults;
        //}

        //public List<string> SearchAll(string query)
        //{
        //    var campaignResults = SearchCampaigns(query);
        //    var brandResults = SearchBrands(query);

        //    var combinedResults = campaignResults.Concat(brandResults).ToList();

        //    // En yüksek skorlarına göre sırala
        //    combinedResults = combinedResults.OrderByDescending(x => FuzzySharp.Fuzz.PartialRatio(query, x.FuzzySearchString)).ToList();

        //    var result = combinedResults.Select(x => x.FuzzySearchString).Take(10).ToList(); // İlk 10 sonucu döndür
        //    return result;
        //}
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
                    MatchedPositionsBrand = GetMatchedPositions(query, BrandFuzzySearchString(campaign.Brand)),
                    DataType = DataType.Campaign
                };
            });
            var campaigns = searches.Select(campaign =>
            {
                return new SearchResultModelV2
                {
                    Campaign = campaign.Campaign,
                    OverallScore = (campaign.CampaignScore + campaign.BrandScore) / 2,
                    MatchedPositionsBrand = campaign.MatchedPositionsBrand,
                    MatchedPositionsCampaign = campaign.MatchedPositionsCampaign,
                    HighlightedCampaign = HighlightMatchedPart(campaign.CampaignSearchString, campaign.MatchedPositionsCampaign),
                    HighlightedBrand = HighlightMatchedPart(campaign.BrandSearchString, campaign.MatchedPositionsBrand),
                    CampaignScore = campaign.CampaignScore,
                    BrandScore = campaign.BrandScore,
                };
            })
                .OrderByDescending(campaign => campaign.OverallScore);
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
