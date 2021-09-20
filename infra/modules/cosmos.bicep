param location string = resourceGroup().location // Location for all resources
param dbName string = 'crgar-cosmosdb2-db'

resource cosmosdbdeployment 'Microsoft.DocumentDB/databaseAccounts@2021-06-15' = {
  name: dbName
  location: location
  kind: 'GlobalDocumentDB'
  properties: {
    backupPolicy: {
      type: 'Continuous'
    }
    connectorOffer: 'Small'
    consistencyPolicy: {
      defaultConsistencyLevel: 'Session'
    }
    enableMultipleWriteLocations: false
    databaseAccountOfferType: 'Standard'
  }
}

output cosmosKey string = listKeys(cosmosdbdeployment.id, cosmosdbdeployment.apiVersion).primaryMasterKey
output cosmosDocumentEndpoint string = cosmosdbdeployment.properties.documentEndpoint
