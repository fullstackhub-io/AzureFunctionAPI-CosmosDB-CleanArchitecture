[assembly: FunctionsStartup(typeof(Startup))]

namespace af_userapi;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"local.settings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        builder.Services.AddSingleton<IConfiguration>(configuration);

        CosmosDbSettings cosmosDbConfig = configuration.GetSection("ConnectionStrings:FamspotDB").Get<CosmosDbSettings>();
        builder.Services.AddInfrastructure(cosmosDbConfig.EndpointUrl,
                             cosmosDbConfig.PrimaryKey,
                             cosmosDbConfig.DatabaseName,
                             cosmosDbConfig.Containers);

        builder.Services.AddScoped<IUserRepository, UserRepository>();

        builder.Services.AddApplication();
    }
}
