using Microsoft.Azure.Cosmos;
using MyApplication.Data;
using MyApplication.Model;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyApplicationIntegrationTests
{
    public class CosmosDbService
    {
        public static int TestDatabaseDocumentsCount { get; private set; }
        
        private static CosmosClient cosmosClientSingleton;
        private static object cosmosClientSingletonLock = new Object();

        public static CosmosClient GetCosmosClient(string EndpointUrl, string AuthorizationKey)
        {
            if (cosmosClientSingleton == null)
            {
                lock (cosmosClientSingletonLock)
                {
                    if (cosmosClientSingleton == null)
                    {
                        CosmosClientOptions cosmosClientOptions = new CosmosClientOptions()
                        {
                            HttpClientFactory = () =>
                            {
                                HttpMessageHandler httpMessageHandler = new HttpClientHandler()
                                {
                                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                                };

                                return new HttpClient(httpMessageHandler);
                            },
                            ConnectionMode = ConnectionMode.Gateway
                        };
                        cosmosClientSingleton = new CosmosClient(EndpointUrl, AuthorizationKey, cosmosClientOptions);
                        LoadDataIntoCosmos(cosmosClientSingleton).Wait();
                    }
                }
            }
            return cosmosClientSingleton;
        }

        private static async Task<Product[]> LoadProductsFromJson()
        {
            Product[] products;
            using (var sr = new StreamReader("TestData/CosmosDbDataLong.json"))
            {
                // Read the stream as a string, and write the string to the console.
                products = await JsonSerializer.DeserializeAsync<Product[]>(sr.BaseStream);
            }
            TestDatabaseDocumentsCount = products.Count();
            return products;
        }
        /// <summary>
        /// TODO: Take a look at this: https://docs.microsoft.com/en-us/azure/cosmos-db/sql/how-to-migrate-from-bulk-executor-library
        /// </summary>
        /// <param name="cosmosClient"></param>
        /// <returns></returns>
        private static async Task LoadDataIntoCosmos(CosmosClient cosmosClient)
        {
            try
            {
                await cosmosClient.GetDatabase(CosmosDbContext.DbName).DeleteAsync();
            }
            catch(Exception)
            {

            }
            var dr = await cosmosClient.CreateDatabaseIfNotExistsAsync(CosmosDbContext.DbName);
            Database database = dr;

            ContainerProperties containerProperties = new ContainerProperties()
            {
                Id = CosmosDbContext.ContainerName,
                PartitionKeyPath = CosmosDbContext.PartitionKeyPath
            };
            Container container = database.CreateContainerIfNotExistsAsync(containerProperties).Result;

            Product[] products = await LoadProductsFromJson();
            List<Task> insertTasks = new List<Task>(products.Count());

            foreach (Product product in products)
            {
                insertTasks.Add(container.CreateItemAsync(product));
            }

            await Task.WhenAll(insertTasks);
        }
    }
}
