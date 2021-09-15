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

