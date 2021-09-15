using Microsoft.Azure.Cosmos;
using MyApplication.Data;
using MyApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApplication
{
    public class PricingService
    {
        public async Task<Product> GetPriceAsync(string productName, CosmosClient client)
        {
            var query = $"SELECT * FROM p WHERE p.Name = '{productName}'";
            Container container = client.GetContainer(CosmosDbContext.DbName, CosmosDbContext.ContainerName);

            QueryDefinition queryDefinition = new QueryDefinition(query);
            FeedIterator<Product> queryResultSetIterator = container.GetItemQueryIterator<Product>(queryDefinition);

            Product product = (await queryResultSetIterator.ReadNextAsync()).First();
            product.Price = CalculateDiscount(product.Price, 12);
            return product;
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
