
namespace user.infrastructure.CosmosDbData.Repository;

public abstract class CosmosDbRepository<T> : IRepository<T>, IContainerContext<T> where T : BaseEntity
{
    /// <summary>
    ///     Generate id
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public abstract string GenerateId(T entity);

    /// <summary>
    ///     Resolve the partition key
    /// </summary>
    /// <param name="entityId"></param>
    /// <returns></returns>
    public abstract PartitionKey ResolvePartitionKey(string entityId);

    /// <summary>
    ///     Generate id for the audit record.
    ///     All entities will share the same audit container,
    ///     so we can define this method here with virtual default implementation.
    ///     Audit records for different entities will use different partition key values,
    ///     so we are not limited to the 20G per logical partition storage limit.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public virtual string GenerateAuditId(Audit entity) => $"{entity.EntityId}:{Guid.NewGuid()}";

    /// <summary>
    ///     Resolve the partition key for the audit record.
    ///     All entities will share the same audit container,
    ///     so we can define this method here with virtual default implementation.
    ///     Audit records for different entities will use different partition key values,
    ///     so we are not limited to the 20G per logical partition storage limit.
    /// </summary>
    /// <param name="entityId"></param>
    /// <returns></returns>
    public virtual PartitionKey ResolveAuditPartitionKey(string entityId) => new PartitionKey($"{entityId.Split(':')[0]}:{entityId.Split(':')[1]}");

    /// <summary>
    ///     Cosmos DB factory
    /// </summary>
    private readonly ICosmosDbContainerFactory _cosmosDbContainerFactory;

    /// <summary>
    ///     Cosmos DB container
    /// </summary>
    private readonly Container _container;
    /// <summary>
    ///     Audit container that will store audit log for all entities.
    /// </summary>
    private readonly Container _auditContainer;

    public CosmosDbRepository(ICosmosDbContainerFactory cosmosDbContainerFactory, string containerName)
    {
        this._cosmosDbContainerFactory = cosmosDbContainerFactory
            ?? throw new ArgumentNullException(nameof(ICosmosDbContainerFactory));
        this._container = this._cosmosDbContainerFactory.GetContainer(containerName)._container;
        this._auditContainer = this._cosmosDbContainerFactory.GetContainer(containerName)._container;
    }

    public async Task AddItemAsync(T item)
    {
        item.Id = GenerateId(item);
        var key = ResolvePartitionKey(item.Id);
        await _container.CreateItemAsync(item, key);
    }

    public async Task DeleteItemAsync(string id)
    {
        await this._container.DeleteItemAsync<T>(id, ResolvePartitionKey(id));
    }

    public async Task<T> GetItemAsync(string id)
    {
        try
        {
            ItemResponse<T> response = await _container.ReadItemAsync<T>(id, ResolvePartitionKey(id));
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<IEnumerable<T>> GetItemsAsync(string queryString)
    {
        FeedIterator<T> resultSetIterator = _container.GetItemQueryIterator<T>(new QueryDefinition(queryString));
        List<T> results = new();
        while (resultSetIterator.HasMoreResults)
        {
            FeedResponse<T> response = await resultSetIterator.ReadNextAsync();

            results.AddRange(response.ToList());
        }

        return results;
    }

    /// <inheritdoc cref="IRepository{T}.GetItemsAsync(ISpecification{T})"/>
    public async Task<IEnumerable<T>> GetItemsAsync(ISpecification<T> specification)
    {
        IQueryable<T> queryable = ApplySpecification(specification);
        FeedIterator<T> iterator = queryable.ToFeedIterator();

        List<T> results = new();
        while (iterator.HasMoreResults)
        {
            FeedResponse<T> response = await iterator.ReadNextAsync();

            results.AddRange(response.ToList());
        }

        return results;
    }

    /// <inheritdoc cref="IRepository{T}.GetItemsCountAsync(ISpecification{T})"/>
    public Task<Response<int>> GetItemsCountAsync(ISpecification<T> specification)
    {
        IQueryable<T> queryable = ApplySpecification(specification);
        return queryable.CountAsync();
    }

    public async Task UpdateItemAsync(string id, T item)
    {
        // Audit
        await Audit(item);
        // Update
        await this._container.UpsertItemAsync<T>(item, ResolvePartitionKey(id));
    }

    /// <summary>
    ///     Evaluate specification and return IQueryable
    /// </summary>
    /// <param name="specification"></param>
    /// <returns></returns>
    private IQueryable<T> ApplySpecification(ISpecification<T> specification)
    {
        CosmosDbSpecificationEvaluator<T> evaluator = new();
        return evaluator.GetQuery(_container.GetItemLinqQueryable<T>(), specification);
    }

    /// <summary>
    ///     Audit a item by adding it to the audit container
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private async Task Audit(T item)
    {
        Audit auditItem = new(item.GetType().Name,
                                                item.Id,
                                                Newtonsoft.Json.JsonConvert.SerializeObject(item));
        auditItem.Id = GenerateAuditId(auditItem);
        await _auditContainer.CreateItemAsync(auditItem, ResolveAuditPartitionKey(auditItem.Id));
    }


}