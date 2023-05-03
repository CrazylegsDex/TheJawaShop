/*
    This class Seeds the database with initial data.

    Date: 05-03-2023
    Author: Dexter Downey
*/

using Microsoft.EntityFrameworkCore;

namespace TheJawaShop.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            // Create a context to the database
            using 
            (
                var context = new DatabaseContext
                (
                    serviceProvider.GetRequiredService<DbContextOptions<DatabaseContext>>()
                )
            )
            {
                // Test if the database already has product data
                if (context.Product.Any()) { return; }

                // Seed the Database with products
                context.Product.AddRange
                (
                    new Product { ProductName = "Jedi Cloak", ProductPrice = 67.25m },
                    new Product { ProductName = "Light Saber Handle", ProductPrice = 100.00m },
                    new Product { ProductName = "Dark Saber", ProductPrice = 25000.00m },
                    new Product { ProductName = "Kyber Crystal Green", ProductPrice = 50000.00m },
                    new Product { ProductName = "Kyber Crystal Blue", ProductPrice = 50000.00m },
                    new Product { ProductName = "Kyber Crystal Purple", ProductPrice = 50000.00m },
                    new Product { ProductName = "Kyber Crystal Red", ProductPrice = 53000.00m },
                    new Product { ProductName = "Beskar Ingots", ProductPrice = 40000.00m },
                    new Product { ProductName = "Thermal Detonator", ProductPrice = 1000.10m },
                    new Product { ProductName = "DT-29 Heavy Blaster Pistol", ProductPrice = 1112.37m },
                    new Product { ProductName = "DL-44 Heavy Blaster Pistol", ProductPrice = 1125.99m },
                    new Product { ProductName = "EE-3 Blaster Rifle", ProductPrice = 1150.0m },
                    new Product { ProductName = "E-5 Blaster Rifle", ProductPrice = 1149.99m },
                    new Product { ProductName = "E-11 Blaster Rifle", ProductPrice = 1134.81m },
                    new Product { ProductName = "E-11D Blaster Rifle", ProductPrice = 1162.15m },
                    new Product { ProductName = "Astromech Droid", ProductPrice = 15000.60m },
                    new Product { ProductName = "Protocol Droid", ProductPrice = 15500.60m },
                    new Product { ProductName = "Battle Droid", ProductPrice = 20150.00m },
                    new Product { ProductName = "Super Battle Droid", ProductPrice = 22250.00m },
                    new Product { ProductName = "IG-100 Magna Guard", ProductPrice = 2300.00m },
                    new Product { ProductName = "Destroyer Droid", ProductPrice = 25000.00m },
                    new Product { ProductName = "AT-ST", ProductPrice = 30000.00m },
                    new Product { ProductName = "AT-TE", ProductPrice = 35000.00m },
                    new Product { ProductName = "AT-AT", ProductPrice = 40000.00m },
                    new Product { ProductName = "X-Wing", ProductPrice = 51000.00m },
                    new Product { ProductName = "Tie Fighter", ProductPrice = 60000.00m },
                    new Product { ProductName = "Slave I", ProductPrice = 73000.00m },
                    new Product { ProductName = "Millenium Falcon", ProductPrice = 75000.00m },
                    new Product { ProductName = "Star Destroyer", ProductPrice = 80000.00m },
                    new Product { ProductName = "Death Star", ProductPrice = 100000.00m }
                );

                // Save the database
                context.SaveChanges();
            }
        }
    }
}