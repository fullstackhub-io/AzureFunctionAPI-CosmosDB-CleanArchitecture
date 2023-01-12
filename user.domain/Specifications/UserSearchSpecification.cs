namespace user.domain.Specifications;

public class UserSearchSpecification : Specification<Entities.User>
{
    public UserSearchSpecification(string id,
                                   int pageStart = 0,
                                   int pageSize = 50,
                                   string sortColumn = "FirstName",
                                   SortDirection sortDirection = SortDirection.Ascending,
                                   bool exactSearch = false)
    {
        if (!string.IsNullOrWhiteSpace(id))
        {
            if (exactSearch)
            {
                Query.Where(item => item.Id.ToLower() == id.ToLower());
            }
            else
            {
                Query.Where(item => item.Id.ToLower().Contains(id.ToLower()));
            }
        }

        // Pagination
        if (pageSize != -1) //Display all entries and disable pagination 
        {
            Query.Skip(pageStart).Take(pageSize);
        }

        // Sort
        switch (sortColumn.ToLower())
        {
            case ("FirstName"):
                {
                    if (sortDirection == SortDirection.Ascending)
                    {
                        Query.OrderBy(x => x.FirstName);
                    }
                    else
                    {
                        Query.OrderByDescending(x => x.FirstName);
                    }
                }
                break;
            default:
                break;
        }
    }
}
