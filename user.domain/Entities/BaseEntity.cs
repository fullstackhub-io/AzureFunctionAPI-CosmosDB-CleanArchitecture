namespace user.domain.Entities;

public abstract class BaseEntity
{
    [JsonProperty(PropertyName = "id")]
    public virtual string? Id { get; set; }
}
