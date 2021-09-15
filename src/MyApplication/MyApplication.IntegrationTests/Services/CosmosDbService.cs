using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MyApplicationIntegrationTests
{
    public class CosmosDbService
    {
        public static CosmosClient GetCosmosClient(string EndpointUrl, string AuthorizationKey)
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
            return new CosmosClient(EndpointUrl, AuthorizationKey, cosmosClientOptions);
        }
    }
}
