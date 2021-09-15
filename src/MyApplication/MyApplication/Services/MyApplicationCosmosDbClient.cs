using Microsoft.Azure.Cosmos;

namespace MyApplication.Services
{
    public class MyApplicationCosmosDbClient: IMyApplicationCosmosDbClient
    {
        public CosmosClient client { get; set; }
    }
}
