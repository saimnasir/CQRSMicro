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

        public IEnumerable<SearchResultModel> SearchAllCampaigns(string query)
        {
            query = query.ToLower();
            IEnumerable<SearchResultModel> campaigns = SearchFromCampaigns(query);

            IEnumerable<SearchResultModel> campaignsOverBrands = SearchFromBrands(query);

            var concatedResults = campaigns.Concat(campaignsOverBrands).ToList();
            var orderedResults = concatedResults.GroupBy(result => result.Campaign.Title)
                .Select(group => group.OrderByDescending(result => result.Score).First())
                .OrderByDescending(result => result.Score);

            var result = orderedResults.Where(brand => brand.Score > 50).Take(10).ToList();
            result.ForEach(item => item.Campaign.Brand = Data.Brands.FirstOrDefault(b => b.Id == item.BrandId));

            return result;
        }

        private static IEnumerable<SearchResultModel> SearchFromBrands(string query)
        {
            var campaigns = Data.Brands
                .Select(brand => new
                {
                    BrandId = brand.Id,
                    Score = Fuzz.WeightedRatio(query, BrandFuzzySearchString(brand)),
                    Campaigns = Data.Campaigns.Where(campaign => campaign.BrandId == brand.Id)
                })
                .SelectMany(brand => brand.Campaigns.Select(campaign => new SearchResultModel
                {
                    Campaign = campaign,
                    BrandId = brand.BrandId,
                    Score = brand.Score,
                    DataType = DataType.Brand
                }))
                .OrderByDescending(result => result.Score);
 
            return campaigns;
        }

        private static IEnumerable<SearchResultModel> SearchFromCampaigns(string query)
        {
            var campaigns = Data.Campaigns.Select(campaign =>
            {
                return new SearchResultModel
                {
                    Campaign = campaign,
                    BrandId = campaign.BrandId,
                    Score = Fuzz.WeightedRatio(query, CampaignFuzzySearchString(campaign), FuzzySharp.PreProcess.PreprocessMode.Full),
                    DataType = DataType.Campaign
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

    }
}
