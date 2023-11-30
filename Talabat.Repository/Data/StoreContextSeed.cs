using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregation;
using Talabat.Repository.Data.Contexts;

namespace Talabat.Repository.Data
{
    public static class StoreDbContextSeed
    {
        public static async Task SeedAsync(StoreDbContext dbContext)
        {
            //Product Brand Seeding
            if (!dbContext.ProductBrands.Any())
            {
                var BrandsData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/brands.json");
                var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandsData);
                if (Brands?.Count > 0)
                {
                    foreach (var Brand in Brands)
                        await dbContext.AddAsync<ProductBrand>(Brand);
                    //await dbContext.SaveChangesAsync();
                }
            }
            

            //Product Type Seeding
            if (!dbContext.ProductTypes.Any())
            {
                var TypesData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/types.json");
                var Types = JsonSerializer.Deserialize<List<ProductType>>(TypesData);
                if (Types?.Count > 0)
                {
                    foreach (var Type in Types)
                        await dbContext.AddAsync<ProductType>(Type);
                    //await dbContext.SaveChangesAsync();
                }
            }
            

            //Products Seeding
            if (!dbContext.Products.Any())
            {
                var ProductsData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/products.json");
                var Products = JsonSerializer.Deserialize<List<Product>>(ProductsData);
                if (Products?.Count > 0)
                {
                    foreach (var product in Products)
                        await dbContext.AddAsync<Product>(product);
                    //await dbContext.SaveChangesAsync();
                }
            }


            //Delivery methods Seeding
            if (!dbContext.DeliveryMethods.Any())
            {
                var DeliveryMethodsData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/delivery.json");
                var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodsData);
                if (DeliveryMethods?.Count > 0)
                {
                    foreach (var Method in DeliveryMethods)
                        await dbContext.AddAsync<DeliveryMethod>(Method);
                    //await dbContext.SaveChangesAsync();
                }
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
