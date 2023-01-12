using User = user.domain.Entities.User;

namespace user.infrastructure.CosmosDbData.Repository;

public class UserRepository : CosmosDbRepository<User>, IUserRepository
{
    /// <summary>
    ///     Generate Id.
    ///     e.g. "shoppinglist:783dfe25-7ece-4f0b-885e-c0ea72135942"
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public override string GenerateId(User entity) => $"{entity.UserType}:{Guid.NewGuid()}";

    /// <summary>
    ///     Returns the value of the partition key
    /// </summary>
    /// <param name="entityId"></param>
    /// <returns></returns>
    public override PartitionKey ResolvePartitionKey(string entityId) => new(entityId.Split(':')[0]);

    public UserRepository(ICosmosDbContainerFactory factory, IConfigConstants configConstants) : base(factory, configConstants.USER_CONTAINER)
    { }

    // Use Cosmos DB Parameterized Query to avoid SQL Injection.
    // Get by Category is also an example of single partition read, where get by title will be a cross partition read
    public async Task<IEnumerable<User>> GetItemsAsyncByID(string Id)
    {
        var results = new List<User>();
        string query = @$"SELECT c.Name FROM c WHERE c.UserID = @Id";

        QueryDefinition queryDefinition = new QueryDefinition(query)
                                                .WithParameter("@Id", Id);
        string queryString = queryDefinition.QueryText;

        var entities = await this.GetItemsAsync(queryString);

        return results;
    }

    // Use Cosmos DB Parameterized Query to avoid SQL Injection.
    // Get by Title is also an example of cross partition read, where Get by Category will be single partition read
    public async Task<IEnumerable<User>> GetItemsAsyncByEmail(string email)
    {
        List<User> results = new();
        string query = @$"SELECT c.Name FROM c WHERE c.Email = @Email";

        QueryDefinition queryDefinition = new QueryDefinition(query)
                                                .WithParameter("@Email", email);
        string queryString = queryDefinition.QueryText;

        IEnumerable<User> entities = await this.GetItemsAsync(queryString);

        return results;
    }
}
