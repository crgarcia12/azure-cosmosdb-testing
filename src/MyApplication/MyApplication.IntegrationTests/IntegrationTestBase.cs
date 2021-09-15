﻿using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MyApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MyApplicationIntegrationTests
{
    public class IntegrationTestBase
    {
        protected readonly HttpClient httpClient;

        public IntegrationTestBase()
        {
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>{
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(CosmosClient));

                        // CosmosDb emulator has hardcoded credentials since it is only supported for local development
                        services.AddSingleton(CosmosDbService.GetCosmosClient("https://localhost:8081/", "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="));
                    });
                });

            this.httpClient = appFactory.CreateClient();
        }
    }
}
