using Microsoft.Azure.Cosmos;
using MyApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApplication
{
    public class PricingService
    {
        public async Task<double> GetPrice(string productName, CosmosClient client)
        {
            var query = $"SELECT * FROM c WHERE c.Name = '${productName}'";
            Container container = client.GetContainer("pricesdb", "productPrices");


            QueryDefinition queryDefinition = new QueryDefinition(query);
            FeedIterator<Product> queryResultSetIterator = container.GetItemQueryIterator<Product>(queryDefinition);

            Product product = (await queryResultSetIterator.ReadNextAsync()).First();

            return CalculateDiscount(product.Price, 12);
        }

        private double CalculateDiscount(double price, int quantity)
        {
            if(quantity > 10)
            {
                return price * 0.8 / quantity * 0.9;
            }
            return price;
        }
    }
}
