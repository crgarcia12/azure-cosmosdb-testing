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
        internal static CosmosClient CosmosClient { get; private set; } = null;

        public void Dispose()
        {
            
        }
        public CosmosDbFixture()
        {
            CosmosClient = CosmosDbService.GetCosmosClient("https://127.0.0.1:8081/", "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");
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
        }
    }
}
