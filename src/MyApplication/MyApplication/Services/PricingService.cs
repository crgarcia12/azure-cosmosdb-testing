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
        public async Task<int> GetProductCountAsync(CosmosClient client)
        {
            var query = $"SELECT VALUE COUNT(1) FROM c";
            Container container = client.GetContainer(CosmosDbContext.DbName, CosmosDbContext.ContainerName);

            QueryDefinition queryDefinition = new QueryDefinition(query);
            FeedIterator<int> queryResultSetIterator = container.GetItemQueryIterator<int>(queryDefinition);

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<int> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                return currentResultSet.First();
            }
            throw new Exception("No data to count?");
        }

        public async Task<Product> GetPriceAsync(string productName, CosmosClient client)
        {
            var query = $"SELECT * FROM p WHERE p.name = '{productName}'";
            Container container = client.GetContainer(CosmosDbContext.DbName, CosmosDbContext.ContainerName);

            QueryDefinition queryDefinition = new QueryDefinition(query);
            FeedIterator<Product> queryResultSetIterator = container.GetItemQueryIterator<Product>(queryDefinition);

            List<Product> products = new List<Product>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Product> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (Product product in currentResultSet)
                {
                    product.price = CalculateDiscount(product.price, 12);
                    products.Add(product);
                }
            }

            return products.First();
        }

        internal double CalculateDiscount(double price, int quantity)
        {
            if(quantity > 10)
            {
                return price * 0.8;
            }
            return price;
        }
    }
}
