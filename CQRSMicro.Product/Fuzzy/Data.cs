using CQRSMicro.Product.Fuzzy.Models;

namespace CQRSMicro.Product.Fuzzy
{
    public static class Data
    {
        public static List<Campaign> Campaigns => new List<Campaign>
        {
            new Campaign { BrandId = 1, Title = "Summer Sale", Description = "Great deals on summer products", Content = "Check out our exclusive summer discounts",  CampaignType = "Seasonal" },
            new Campaign { BrandId = 2, Title = "Back to School", Description = "Special offers on school supplies", Content = "Get ready for the new school year with our amazing deals",  CampaignType = "Seasonal" },
            new Campaign { BrandId = 3, Title = "Holiday Extravaganza", Description = "Celebrate the holidays with us", Content = "Unwrap the joy of festive discounts and promotions",  CampaignType = "Seasonal" },
            new Campaign { BrandId = 4, Title = "Black Friday Madness", Description = "Don't miss out on our Black Friday deals", Content = "Get incredible discounts on a wide range of products", CampaignType = "Seasonal" },
            new Campaign { BrandId = 5, Title = "Tech Expo", Description = "Explore the latest in technology", Content = "Join us for a showcase of cutting-edge tech innovations",   CampaignType = "Event" },
            new Campaign { BrandId = 1, Title = "Spring Fashion Showcase", Description = "Discover the latest trends in spring fashion", Content = "Get ready to revamp your wardrobe with our new collection",  CampaignType = "Fashion" },
            new Campaign { BrandId = 2, Title = "Gaming Marathon", Description = "Calling all gamers to the ultimate gaming marathon", Content = "Experience non-stop gaming action and exclusive deals",   CampaignType = "Gaming" },
            new Campaign { BrandId = 3, Title = "Home Decor Extravaganza", Description = "Transform your home with our exclusive home decor items", Content = "Find the perfect pieces to enhance your living space", CampaignType = "Home Decor" },
            new Campaign { BrandId = 4, Title = "Fall Fitness Challenge", Description = "Achieve your fitness goals with our special challenge", Content = "Join us for a season of health and wellness", CampaignType = "Health" },
            new Campaign { BrandId = 5, Title = "Adventure Travel Deals", Description = "Embark on an adventure with our exclusive travel offers", Content = "Explore new destinations with exciting travel packages", CampaignType = "Travel" },
            new Campaign { BrandId = 5, Title = "Movie Night Extravaganza Finance", Description = "Enjoy movie nights with our blockbuster deals", Content = "Grab your popcorn and get ready for a cinematic experience", CampaignType = "Entertainment" },
            new Campaign { BrandId = 6, Title = "Winter Wellness Festival", Description = "Embrace the winter season with our wellness festival", Content = "Discover winter wellness tips and exclusive offers", CampaignType = "Health" },
            new Campaign { BrandId = 7, Title = "Smart Home Showcase", Description = "Upgrade your home with the latest smart home technology", Content = "Experience the convenience of a smart home", CampaignType = "Technology" },
            new Campaign { BrandId = 3, Title = "Pet Lovers' Paradise", Description = "Spoil your pets with our special pet-friendly deals", Content = "Treat your furry friends to the best in pet products", CampaignType = "Pets" },
            new Campaign { BrandId = 3, Title = "Bookworm's Delight", Description = "Indulge in a world of books with our literary offers", Content = "Discover captivating stories and literary treasures", CampaignType = "Books" },
            new Campaign { BrandId = 3, Title = "DIY Home Improvement Week", Description = "Transform your space with DIY home improvement projects", Content = "Get creative and upgrade your living space", CampaignType = "Home Improvement" },
            new Campaign { BrandId = 6, Title = "Music Festival Flash Sale", Description = "Experience the magic of music with our festival flash sale", Content = "Get exclusive discounts on musical instruments and accessories", CampaignType = "Music" },
            new Campaign { BrandId = 8, Title = "Green Living Expo", Description = "Discover sustainable living with our green expo", Content = "Explore eco-friendly products and sustainable lifestyle choices", CampaignType = "Environment" },
            new Campaign { BrandId = 8, Title = "Fitness Tracker Finance", Description = "Stay fit with our latest fitness tracker launch", Content = "Track your fitness goals with advanced fitness technology", CampaignType = "Health" },
            new Campaign { BrandId = 8, Title = "Summer Movie Nights", Description = "Enjoy summer nights with outdoor movie experiences", Content = "Grab your blankets and join us for open-air cinema", CampaignType = "Entertainment" },

        };

        public static List<Brand> Brands => new List<Brand>
        {
            new Brand { Id= 1, Name = "ABC Tech", Sector = "Technology", About = "Innovative technology solutions",  BrandType = "Electronics" },
            new Brand { Id= 2, Name = "XYZ Fashion", Sector = "Apparel", About = "Trendy and stylish clothing",   BrandType = "Fashion" },
            new Brand { Id= 3, Name = "SuperFood", Sector = "Food and Beverage", About = "Healthy and delicious food options", BrandType = "Food" },
            new Brand { Id= 4, Name = "Global Motors", Sector = "Automotive", About = "Driving innovation in the automotive industry", BrandType = "Automotive" },
            new Brand { Id= 5, Name = "Finance Wizard", Sector = "Finance", About = "Your trusted financial partner", BrandType = "Finance" },
            new Brand { Id= 6, Name = "Cosmic Entertainment", Sector = "Entertainment", About = "Bringing joy and excitement to your life", BrandType = "Entertainment" },
            new Brand { Id= 7, Name = "Adventure Travels", Sector = "Travel", About = "Discover new horizons with us", BrandType = "Travel" },
            new Brand { Id= 8, Name = "EduTech Solutions", Sector = "Education", About = "Empowering education through technology", BrandType = "Education" },

        };
    }
}
