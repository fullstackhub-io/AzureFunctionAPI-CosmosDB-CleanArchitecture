using User = user.domain.Entities.User;

namespace user.domain.Specifications;

public class UserGetSingleSpecification : Specification<User>
{
    public UserGetSingleSpecification(string email)
    {
        Query.Where(item => item.Email == email);
    }
}