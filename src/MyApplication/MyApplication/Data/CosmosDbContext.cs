using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApplication.Data
{
    public class CosmosDbContext
    {
        public static string DbName = "PricesDb";
        public static string ContainerName = "PricesContainer";
        public static string PartitionKeyPath = "/name";

    }
}
