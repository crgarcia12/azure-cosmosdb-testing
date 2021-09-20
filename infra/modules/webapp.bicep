param location string = resourceGroup().location // Location for all resources
param webappName string = 'crgar-cosmosdb2-webapp'
var appServicePlanName = toLower('${webappName}-plan')
param sku string = 'P1V2' // The SKU of App Service Plan
param cosmosDocumentEndpoint string
param cosmosKey string
param databaseName string = 'PricesDb'
param containerName string = 'Container'

resource appServicePlan 'Microsoft.Web/serverfarms@2020-06-01' = {
  name: appServicePlanName
  location: location
  properties: {
    reserved: true
  }
  sku: {
    name: sku
  }
  kind: 'windows'
}

resource appService 'Microsoft.Web/sites@2020-06-01' = {
  name: webappName
  location: location
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
    siteConfig: {
      http20Enabled: true
      appSettings: [
        {
          name: 'CosmosDb_DocumentEndpoint'
          value: cosmosDocumentEndpoint
        }
        {
          name: 'CosmosDb_Key'
          value: cosmosKey
        }
        {
          name: 'CosmosDb_DatabaseName'
          value: databaseName
        }
        {
          name: 'CosmosDb_ContainerName'
          value: containerName
        }
      ]
    }
  }



}
