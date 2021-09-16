using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Azure.Cosmos;
using MyApplication;
using MyApplication.Model;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xunit;

namespace MyApplicationIntegrationTests
{
    public class PricesTests: IClassFixture<CosmosDbFixture>
    {
        private CosmosDbFixture cosmosDbFixture;

        public PricesTests(CosmosDbFixture cosmosDbFixture)
        {
            this.cosmosDbFixture = cosmosDbFixture;
        }

        [Fact]
        public async Task PriceCalculation_Returns200()
        {
            // Arrange

            // Action
            var response = await this.cosmosDbFixture.TestHttpClient.GetAsync("/api/Prices");

            // Assert
            Assert.Equal(response.StatusCode, System.Net.HttpStatusCode.OK);

        }

        [Fact]
        public async Task PriceCalculation_MakesADiscount()
        {
            // Arrange

            // Action
            var response = await this.cosmosDbFixture.TestHttpClient.GetAsync("/api/Prices/ProductOne");

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            string jsonResponse = await response.Content.ReadAsStringAsync();
            var product = JsonSerializer.Deserialize<Product>(jsonResponse, options);


            // Assert
            Assert.Equal(response.StatusCode, System.Net.HttpStatusCode.OK);
            Assert.InRange(product.price, 0.07, 0.075);
        }

        [Fact]
        public async Task PriceCalculation_CanHandleCeroPrice()
        {
            // Arrange

            // Action
            var response = await this.cosmosDbFixture.TestHttpClient.GetAsync("/api/Prices/ProductCero");

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            string jsonResponse = await response.Content.ReadAsStringAsync();
            var product = JsonSerializer.Deserialize<Product>(jsonResponse, options);

            // Assert
            Assert.Equal(response.StatusCode, System.Net.HttpStatusCode.OK);
            Assert.Equal(product.price, 0);
        }
    }
}
