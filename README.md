[![Build Status](https://dev.azure.com/crgarcia/azure-cosmosdb-testing/_apis/build/status/crgarcia12.azure-cosmosdb-testing?branchName=main)](https://dev.azure.com/crgarcia/azure-cosmosdb-testing/_build/latest?definitionId=45&branchName=main)

# azure-cosmosdb-testing
How to test cosmosDB? let's play a bit

## DotNet settings.
Assuming you have two DBs - one Dev for local development and one for testing

```
cd src/MyApplication
dotnet user-settings init
dotnet user-settings set "CosmosDb:AuthorizationKey" "<your dev cosmos db>"
```

## Integration tests
Here we are using XUnit

## Running the CosmosDb emulator
Create the DB in WSL2
```
 ipaddr="`ifconfig | grep "inet " | grep -Fv 127.0.0.1 | awk '{print $2}' | head -n 1`"
 
 docker run \
 -p 8081:8081 -p 10251:10251 -p 10252:10252 -p 10253:10253 -p 10254:10254  \
 -m 3g \
 --cpus=2.0 \
 --name=test-linux-emulator \
 -e AZURE_COSMOS_EMULATOR_PARTITION_COUNT=10 \
 -e AZURE_COSMOS_EMULATOR_ENABLE_DATA_PERSISTENCE=true \
 -e AZURE_COSMOS_EMULATOR_IP_ADDRESS_OVERRIDE=$ipaddr \
 -it \
 mcr.microsoft.com/cosmosdb/linux/azure-cosmos-emulator
```

Kill the DB in WSL2
```
  docker kill test-linux-emulator
  docker rm test-linux-emulator
```

Open the Cosmos Data explorer
https://localhost:8081/_explorer/index.html
