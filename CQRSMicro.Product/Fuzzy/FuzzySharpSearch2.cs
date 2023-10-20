using CQRSMicro.Product.Fuzzy.Models;
using FuzzySharp;

namespace CQRSMicro.Product.Fuzzy
{
    public class FuzzySharpSearch2
    {
        public List<FuzzyItem> SearchCampaigns(string query)
        {
            var campaignFuzzyItems = Data.Campaigns.Select(c => new FuzzyItem { Text = c.Title, SearchString = $"{c.Title} {c.Description} {c.Content}" }).ToList();
             ProcessSearchQuery(query, campaignFuzzyItems);
            return campaignFuzzyItems.Select(x => x).Take(10).ToList(); // İlk 10 sonucu döndür
        }

        public List<FuzzyItem> SearchBrands(string query)
        {
            var campaignFuzzyItems = Data.Brands.Select(c => new FuzzyItem { Text = c.Name, SearchString = $"{c.Name} {c.Sector}" }).ToList();
            ProcessSearchQuery(query, campaignFuzzyItems);
            return campaignFuzzyItems.Select(x => x).Take(10).ToList(); // İlk 10 sonucu döndür
        }

        public void ProcessSearchQuery(string query, List<FuzzyItem> items)
        {
            foreach (var item in items)
            {
                item.Sscore = Fuzz.PartialRatio(query, item.SearchString);
            }
            // En yüksek skorları alarak sırala
            items = items.OrderByDescending(x => x.Sscore).ToList();
        }

        public List<FuzzyItem> SearchAll(string query)
        {
            var campaignResults = SearchCampaigns(query);
            var brandResults = SearchBrands(query);

            var combinedResults = campaignResults.Concat(brandResults).ToList();

            // En yüksek skorlarına göre sırala
            combinedResults = combinedResults.OrderByDescending(x => Fuzz.PartialRatio(query, x.SearchString)).ToList();

            var result = combinedResults.Take(10).ToList(); // İlk 10 sonucu döndür
            return result;
        }
    }
}
