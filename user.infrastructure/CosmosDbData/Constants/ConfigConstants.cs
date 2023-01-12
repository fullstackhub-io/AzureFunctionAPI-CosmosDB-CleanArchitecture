namespace user.infrastructure.CosmosDbData.Constants;

public class ConfigConstants : IConfigConstants
{
    public IConfiguration Configuration { get; }
    private readonly CosmosDbSettings cosmosDbConfig;

    public ConfigConstants(IConfiguration configuration)
    {
        this.Configuration = configuration;
        this.cosmosDbConfig = this.Configuration.GetSection("ConnectionStrings:FamspotDB").Get<CosmosDbSettings>();
    }

    public string AUDIT_CONTAINER => this.cosmosDbConfig?.Containers[0]?.Name;

    public string USER_CONTAINER => this.cosmosDbConfig?.Containers[1]?.Name;
}
