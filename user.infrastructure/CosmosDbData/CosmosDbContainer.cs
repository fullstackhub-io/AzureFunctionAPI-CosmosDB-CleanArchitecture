namespace user.infrastructure.CosmosDbData;

public class CosmosDbContainer : ICosmosDbContainer
{
    public Container _container { get; }

    public CosmosDbContainer(CosmosClient cosmosClient,
                             string databaseName,
                             string containerName)
    {
        this._container = cosmosClient.GetContainer(databaseName, containerName);
    }
}