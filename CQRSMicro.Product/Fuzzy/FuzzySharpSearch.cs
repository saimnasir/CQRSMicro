using CQRSMicro.Product.Fuzzy.Models;
using FuzzySharp;

namespace CQRSMicro.Product.Fuzzy
{
    public class FuzzySharpSearch
    {
        public List<Tuple<string, int>> SearchCampaigns(string query)
        {
           // var campaignStrings =Data.Campaigns.Select(c => $"{c.Title} {c.Description}").ToList();
             var campaignStrings =Data.Campaigns.Select(c => $"{c.Title} {c.Description} {c.Content} {c.CampaignType} {c.Content}").ToList();
            var results = ProcessSearchQuery(query, campaignStrings);
            return results.Select(x => x).Take(10).ToList(); // İlk 10 sonucu döndür
        }

        public List<Tuple<string, int>> SearchBrands(string query)
        {
            // var brandStrings = Data.Brands.Select(b => $"{b.Name} {b.Sector}").ToList();
            var brandStrings = Data.Brands.Select(b => $"{b.Name} {b.Sector} {b.About} {b.BrandType}").ToList();
            var results = ProcessSearchQuery(query, brandStrings);
            return results.Select(x => x).Take(10).ToList(); // İlk 10 sonucu döndür
        }

        public List<Tuple<string, int>> ProcessSearchQuery(string query, List<string> items)
        {
            var searchResults = new List<Tuple<string, int>>();

            foreach (var item in items)
            {
                var score = Fuzz.PartialRatio(query, item);
                searchResults.Add(new Tuple<string, int>(item, score));
            }

            // En yüksek skorları alarak sırala
            searchResults = searchResults.OrderByDescending(x => x.Item2).ToList();

            return searchResults;
        }

        public List<string> SearchAll(string query)
        {
            var campaignResults = SearchCampaigns(query);
            var brandResults = SearchBrands(query);

            var combinedResults = campaignResults.Concat(brandResults).ToList();

            // En yüksek skorlarına göre sırala
            combinedResults = combinedResults.OrderByDescending(x => FuzzySharp.Fuzz.PartialRatio(query, x.Item1)).ToList();

            var result = combinedResults.Select(x => x.Item1).Take(10).ToList(); // İlk 10 sonucu döndür
            return result;
        }
    }
}
