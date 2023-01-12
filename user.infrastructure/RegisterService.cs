using user.infrastructure.CosmosDbData.Constants;

namespace user.infrastructure;

public static class RegisterService
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
                                                            string endpointUrl,
                                                            string primaryKey,
                                                            string databaseName,
                                                            List<ContainerInfo> containers)
    {
        var client = new CosmosClient(endpointUrl, primaryKey);
        var cosmosDbClientFactory = new CosmosDbContainerFactory(client, databaseName, containers);
        services.AddSingleton<ICosmosDbContainerFactory>(cosmosDbClientFactory);
        services.AddSingleton<IConfigConstants, ConfigConstants>();
        return services;
    }
}
