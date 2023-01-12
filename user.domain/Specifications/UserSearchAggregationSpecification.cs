using User = user.domain.Entities.User;

namespace user.domain.Specifications;

public class UserSearchAggregationSpecification : Specification<User>
{
    public UserSearchAggregationSpecification(string email = "",
                                         int pageStart = 0,
                                         int pageSize = 50,
                                         string sortColumn = "title",
                                         SortDirection sortDirection = SortDirection.Ascending,
                                         bool exactSearch = false
                                         )
    {
        if (!string.IsNullOrWhiteSpace(email))
        {
            if (exactSearch)
            {
                Query.Where(item => item.Email.ToLower() == email.ToLower());
            }
            else
            {
                Query.Where(item => item.Email.ToLower().Contains(email.ToLower()));
            }
        }

    }
}