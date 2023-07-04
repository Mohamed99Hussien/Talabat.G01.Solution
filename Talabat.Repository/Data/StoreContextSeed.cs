using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Talabat.Core.Entities;
using Microsoft.Extensions.Logging;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context ,ILoggerFactory loggerFactory)
        {
            try
            {
                if (!context.productBrands.Any())
                {

                    var brandsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
                    // convert Json file
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                    foreach (var brand in brands)
                        context.Set<ProductBrand>().Add(brand);
                }

                if (!context.productTypes.Any())
                {

                    var typesData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/types.json");
                    // convert Json file
                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                    foreach (var type in types)
                        context.Set<ProductType>().Add(type);
                }

                if (!context.products.Any())
                {

                    var productsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
                    // convert Json file
                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                    foreach (var product in products)
                        context.Set<Product>().Add(product);
                }

                if (!context.deliveryMethods.Any())
                {

                    var deliveryMethodsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/delivery.json");
                    // convert Json file
                    var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodsData);
                    foreach (var deliveryMethod in deliveryMethods)
                        context.Set<DeliveryMethod>().Add(deliveryMethod);
                }

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                var logger = loggerFactory.CreateLogger<StoreContextSeed>();
                logger.LogError(ex, ex.Message);
                
            }
           
            
         }
    }
}
