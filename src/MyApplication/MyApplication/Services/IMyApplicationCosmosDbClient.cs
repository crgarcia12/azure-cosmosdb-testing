using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApplication.Services
{
    public interface IMyApplicationCosmosDbClient
    {
        public CosmosClient client { get; set; }
    }
}
