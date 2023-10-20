using CQRSMicro.Product.Fuzzy.Models;

namespace CQRSMicro.Product.Fuzzy
{
    public static class Data
    {
        public static List<Campaign> Campaigns => new List<Campaign>
        {
            new Campaign
            {
                CampaignType = "t1 x1",
                Content = "c1 t1 x1",
                Description = "x1",
                Title = "x1",
                CreatedAt = DateTime.UtcNow
            }
        };

        public static List<Brand> Brands => new List<Brand>
        {
            new Brand
            {
                Name = "b1 x1",
                About = "a1 b1 x1 ",
                Sector = "s1 x1",
                BrandType = "b1 t1",
                CreatedAt = DateTime.UtcNow
            }
        };
    }
}
