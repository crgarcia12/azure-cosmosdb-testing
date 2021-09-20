param environmentName string = 'cosmostest1'

module cosmosDB 'modules/cosmos.bicep' = {
  name: 'cosmosDeploy'
  params : {
    dbName: '${environmentName}-db'
  }
}

module webApp 'modules/webapp.bicep' = {
  name: 'webAppDeploy'
  params: {
    webappName: '${environmentName}-app'
    cosmosDocumentEndpoint : cosmosDB.outputs.cosmosDocumentEndpoint
    cosmosKey: cosmosDB.outputs.cosmosKey
  }
}
