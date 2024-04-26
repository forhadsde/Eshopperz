using API.Models;
using Microsoft.AspNetCore.Identity;

namespace API.Data;

public static class DbInitializer
{
    public static async Task Initialize(StoreContext context, UserManager<User> userManager)
    {
        if (!userManager.Users.Any())
        {
            var user = new User
            {
                UserName = "bob",
                Email = "bob@test.com"
            };

            await userManager.CreateAsync(user, "Pa$$w0rd");
            await userManager.AddToRoleAsync(user, "Member");

            var admin = new User
            {
                UserName = "admin",
                Email = "admin@test.com"
            };

            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRolesAsync(admin, new[] { "Admin", "Member" });
        }

        if (context.Products.Any()) return;

        var products = new List<Product>
        {
                new Product
    {
        Name = "Maybelline New York Liquid Foundation Makeup",
        Description = "Matte foundation for oily skin.",
        Price = 799,
        PictureUrl = "/images/products/1.png",
        Brand = "Maybelline New York",
        Type = "Cosmetics",
        QuantityInStock = 50
    },
    new Product
    {
        Name = "Urban Decay Naked Eyeshadow Palette",
        Description = "Highly pigmented eyeshadow palette.",
        Price = 5400,
        PictureUrl = "/images/products/2.jpeg",
        Brand = "Urban Decay",
        Type = "Cosmetics",
        QuantityInStock = 30
    },
    
    // Medicine
    new Product
    {
        Name = "Tylenol Extra Strength Acetaminophen",
        Description = "Pain reliever and fever reducer.",
        Price = 899,
        PictureUrl = "/images/products/3.png",
        Brand = "Tylenol",
        Type = "Medicine",
        QuantityInStock = 100
    },
    new Product
    {
        Name = "Benadryl Allergy Ultratabs",
        Description = "Antihistamine for allergy relief.",
        Price = 749,
        PictureUrl = "/images/products/4.png",
        Brand = "Benadryl",
        Type = "Medicine",
        QuantityInStock = 80
    },
    
    // Jewelry
    new Product
    {
        Name = "Tiffany & Co. Diamond Stud Earrings",
        Description = "Classic diamond earrings in platinum.",
        Price = 100000,
        PictureUrl = "/images/products/5.jpeg",
        Brand = "Tiffany & Co.",
        Type = "Jewelry",
        QuantityInStock = 20
    },
    new Product
    {
        Name = "Cartier Love Bracelet",
        Description = "Iconic love bracelet in 18k gold.",
        Price = 700000,
        PictureUrl = "/images/products/6.jpg",
        Brand = "Cartier",
        Type = "Jewelry",
        QuantityInStock = 15
    },
    
    // Shampoo
    new Product
    {
        Name = "Pantene Pro-V Moisture Renewal Shampoo",
        Description = "Hydrating shampoo for dry hair.",
        Price = 599,
        PictureUrl = "/images/products/7.jpeg",
        Brand = "Pantene",
        Type = "Shampoo",
        QuantityInStock = 70
    },
    new Product
    {
        Name = "Head & Shoulders Clean Dandruff Shampoo",
        Description = "Anti-dandruff shampoo for all hair types.",
        Price = 649,
        PictureUrl = "/images/products/8.jpeg",
        Brand = "Head & Shoulders",
        Type = "Shampoo",
        QuantityInStock = 60
    },
    
    // Shower Gel
    new Product
    {
        Name = "Dove Deep Moisture Nourishing Body Wash",
        Description = "Moisturizing body wash for soft skin.",
        Price = 799,
        PictureUrl = "/images/products/9.png",
        Brand = "Dove",
        Type = "Shower Gel",
        QuantityInStock = 40
    },
    new Product
    {
        Name = "The Body Shop Satsuma Shower Gel",
        Description = "Refreshing shower gel with citrus scent.",
        Price = 1200,
        PictureUrl = "/images/products/10.jpeg",
        Brand = "The Body Shop",
        Type = "Shower Gel",
        QuantityInStock = 35
    },
    
    // Perfume
    new Product
    {
        Name = "Chanel Coco Mademoiselle Eau de Parfum",
        Description = "Classic floral perfume with a modern twist.",
        Price = 10000,
        PictureUrl = "/images/products/11.jpeg",
        Brand = "Chanel",
        Type = "Perfume",
        QuantityInStock = 25
    },
    new Product
    {
        Name = "Dolce & Gabbana Light Blue Eau de Toilette",
        Description = "Fresh and fruity fragrance for women.",
        Price = 8000,
        PictureUrl = "/images/products/12.jpg",
        Brand = "Dolce & Gabbana",
        Type = "Perfume",
        QuantityInStock = 30
    },
        new Product
    {
        Name = "MAC Ruby Woo Matte Lipstick",
        Description = "Classic red matte lipstick.",
        Price = 1800,
        PictureUrl = "/images/products/13.jpeg",
        Brand = "MAC",
        Type = "Cosmetics",
        QuantityInStock = 40
    },
    
    // Additional Medicine
    new Product
    {
        Name = "Advil Ibuprofen Pain Reliever/Fever Reducer",
        Description = "Fast-acting pain relief tablets.",
        Price = 899,
        PictureUrl = "/images/products/14.png",
        Brand = "Advil",
        Type = "Medicine",
        QuantityInStock = 90
    },
    
    // Additional Jewelry
    new Product
    {
        Name = "David Yurman Classics Bracelet with Gold",
        Description = "Iconic cable bracelet with gold accents.",
        Price = 150000,
        PictureUrl = "/images/products/15.jpeg",
        Brand = "David Yurman",
        Type = "Jewelry",
        QuantityInStock = 25
    },
    
    // Additional Shampoo
    new Product
    {
        Name = "Herbal Essences Moisturizing Shampoo",
        Description = "Moisturizing shampoo with coconut essence.",
        Price = 499,
        PictureUrl = "/images/products/16.jpeg",
        Brand = "Herbal Essences",
        Type = "Shampoo",
        QuantityInStock = 80
    },
    
    // Additional Shower Gel
    new Product
    {
        Name = "NIVEA Men Active Clean Body Wash",
        Description = "Deep-cleansing body wash for men.",
        Price = 699,
        PictureUrl = "/images/products/17.png",
        Brand = "NIVEA",
        Type = "Shower Gel",
        QuantityInStock = 50
    },
    
    // Additional Perfume
    new Product
    {
        Name = "Yves Saint Laurent Black Opium Eau de Parfum",
        Description = "Addictive floral fragrance with coffee and vanilla.",
        Price = 12000,
        PictureUrl = "/images/products/18.jpeg",
        Brand = "Yves Saint Laurent",
        Type = "Perfume",
        QuantityInStock = 35
    }

        };

        foreach (var product in products)
        {
            context.Products.Add(product);
        }

        context.SaveChanges();
    }
}
