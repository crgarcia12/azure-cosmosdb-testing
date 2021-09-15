using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Azure.Cosmos;
using MyApplication;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace MyApplicationIntegrationTests
{
    public class PricesTests: IntegrationTestBase
    {
        [Fact]
        public async Task TestPriceCAlculation()
        {
            // Arrange

            // Action
            var response = await this.testHttpClient.GetAsync("/api/Prices");

            // Assert
            Assert.Equal(response.StatusCode, System.Net.HttpStatusCode.OK);

        }
    }
}
