//using CQRSMicro.Product.Fuzzy.Models;
//using FuzzyStrings;
//namespace CQRSMicro.Product.Fuzzy
//{
//    public class FuzzyStringSearch
//    {
//        private const int FuzzySearchThreshold = 80;
//        public object SearchAll(string key)
//        {
            
//        }


//        public List<Campaign> SearchCampaigns(string searchTerm)
//        {
//            return Data.Campaigns
//                .Where(campaign =>
//                    campaign.Title.DiceCoefficient(searchTerm) > 0.6 || // You can adjust the similarity threshold
//                    campaign.Description.DiceCoefficient(searchTerm) > 0.6 ||
//                    campaign.Content.DiceCoefficient(searchTerm) > 0.6)
//                .ToList();
//        }

//        public List<Brand> SearchBrands(string searchTerm)
//        {
//            return Data.Brands
//                .Where(brand =>
//                    brand.Name.DiceCoefficient(searchTerm) > 0.6 || // You can adjust the similarity threshold
//                    brand.Sector.DiceCoefficient(searchTerm) > 0.6)
//                .ToList();
//        }
//    }
//}
