using CQRSMicro.Product.Fuzzy.Models;

namespace CQRSMicro.Product.Fuzzy
{
    public static class Data
    {
        public static List<Campaign> Campaigns => new List<Campaign>
        {
            new Campaign
            {
                CampaignType = "Bonus",
                Content = "Arçelik Beko beko2 Bonus",
                Description = "Arçelikten Bekodan beko2den Bonus ",
                Title = "Arçeliğe Bekoya beko2ya Bonus",
                CreatedAt = DateTime.UtcNow
            }
        };

        public static List<Brand> Brands => new List<Brand>
        {
            new Brand
            {
                Name = "Arçelik",
                About = "Arçelik",
                Sector = "Arçelik",
                BrandType = "Arçelik",
                CreatedAt = DateTime.UtcNow
            },
            new Brand
            {
                Name = "Beko",
                About = "Beko",
                Sector = "Beko",
                BrandType = "Beko",
                CreatedAt = DateTime.UtcNow
            },
            new Brand
            {
                Name = "Beko2",
                About = "Beko2",
                Sector = "Beko2",
                BrandType = "Beko",
                CreatedAt = DateTime.UtcNow
            }
        };
    }
}
