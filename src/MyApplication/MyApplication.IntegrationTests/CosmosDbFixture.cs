using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MyApplication;
using MyApplication.Data;
using MyApplication.Model;
using System;
using System.Net.Http;


namespace MyApplicationIntegrationTests
{
    public class CosmosDbFixture: IDisposable
    {
        public HttpClient TestHttpClient { get; private set; }
        internal CosmosClient CosmosClient { get; private set; }

        private void InsertDummyData(CosmosClient cosmosClient)
        {
            Database database = cosmosClient.CreateDatabaseIfNotExistsAsync(CosmosDbContext.DbName).Result;

            ContainerProperties containerProperties = new ContainerProperties() 
            {
                Id = CosmosDbContext.ContainerName,
                PartitionKeyPath = CosmosDbContext.PartitionKeyPath
            };
            Container container = database.CreateContainerIfNotExistsAsync(containerProperties).Result;

            var p1 = new Product()
            {
                id = Guid.NewGuid(),
                name = "ProductOne",
                price = 8
            };
            var p2 = new Product()
            {
                id = Guid.NewGuid(),
                name = "ProductTwo",
                price = 12
            };
            var p3 = new Product()
            {
                id = Guid.NewGuid(),
                name = "ProductCero",
                price = 0
            };
            container.CreateItemAsync(p1).Wait();
            container.CreateItemAsync(p2).Wait();
            container.CreateItemAsync(p3).Wait();
        }

        private void CleanDummyData(CosmosClient cosmosClient)
        {
            cosmosClient.GetDatabase(CosmosDbContext.DbName).DeleteAsync();
        }

        public void Dispose()
        {
            CleanDummyData(this.CosmosClient);
        }

        public CosmosDbFixture()
        {
            this.CosmosClient = CosmosDbService.GetCosmosClient("https://localhost:8081/", "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>{
                    builder.ConfigureServices(services =>
                    {
                        // CosmosDb emulator has hardcoded credentials since it is only supported for local development
                        services.RemoveAll(typeof(CosmosClient));
                        services.AddSingleton(CosmosClient);
                    });
                });

            this.TestHttpClient = appFactory.CreateClient();

            InsertDummyData(CosmosClient);
        }
    }
}
