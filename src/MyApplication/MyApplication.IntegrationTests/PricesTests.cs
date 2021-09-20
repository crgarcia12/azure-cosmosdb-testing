using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Azure.Cosmos;
using MyApplication;
using MyApplication.Model;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xunit;

namespace MyApplicationIntegrationTests
{
    [Collection("CosmosDbCollection")]
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
            string responeText = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(response.StatusCode, System.Net.HttpStatusCode.OK);

        }

        [Fact]
        public async Task PriceCalculation_MakesADiscount()
        {
            // Arrange

            // Action
            var response = await this.cosmosDbFixture.TestHttpClient.GetAsync("/api/Prices/Product1");
            string jsonResponse = await response.Content.ReadAsStringAsync();
            var product = JsonSerializer.Deserialize<Product>(jsonResponse);

            // Assert
            Assert.Equal(response.StatusCode, System.Net.HttpStatusCode.OK);
            Assert.InRange(product.price, 0.00, 900);
        }

        [Fact]
        public async Task PriceCalculation_CanHandleCeroPrice()
        {
            // Arrange
            string productName = "Product1";

            // Action
            HttpResponseMessage response = await this.cosmosDbFixture.TestHttpClient.GetAsync("/api/Prices/" + productName);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            string jsonResponse = await response.Content.ReadAsStringAsync();
            var product = JsonSerializer.Deserialize<Product>(jsonResponse, options);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(productName, product.name);
        }

        [Fact]
        public async Task TestTotalCountOfProductsInTestDatabase()
        {
            HttpResponseMessage response = await this.cosmosDbFixture.TestHttpClient.GetAsync("/api/Prices");

            string returnMessage = await response.Content.ReadAsStringAsync();
            int countOfRecordsInDb = Int32.Parse(returnMessage);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(CosmosDbService.TestDatabaseDocumentsCount, countOfRecordsInDb);

        }
    }
}
