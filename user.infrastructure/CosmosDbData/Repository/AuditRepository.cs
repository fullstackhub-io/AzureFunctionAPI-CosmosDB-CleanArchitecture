namespace user.infrastructure.CosmosDbData.Repository;

public class AuditRepository : CosmosDbRepository<Audit>, IAuditRepository
{
    public override string GenerateId(Audit entity) => GenerateAuditId(entity);
    public override PartitionKey ResolvePartitionKey(string entityId) => ResolveAuditPartitionKey(entityId);

    public AuditRepository(ICosmosDbContainerFactory factory, IConfigConstants configConstants) : base(factory, configConstants.AUDIT_CONTAINER)
    { }

}