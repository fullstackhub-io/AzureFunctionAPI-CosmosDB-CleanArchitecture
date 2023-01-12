namespace user.infrastructure.CosmosDbData.Interfaces;

/// <summary>
///  Defines the container level context
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IContainerContext<T> where T : BaseEntity
{
    //string ContainerName { get; }
    string GenerateId(T entity);
    PartitionKey ResolvePartitionKey(string entityId);
}
