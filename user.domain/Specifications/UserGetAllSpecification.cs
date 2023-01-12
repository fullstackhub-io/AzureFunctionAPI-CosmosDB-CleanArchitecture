using User = user.domain.Entities.User;

namespace user.domain.Specifications;

public class UserGetAllSpecification : Specification<User>
{
    public UserGetAllSpecification(UserStatus status)
    {
        Query.Where(item =>
            item.Status == status
        );
    }
}