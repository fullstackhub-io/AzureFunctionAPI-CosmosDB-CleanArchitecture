namespace user.application.Common.BaseClass;

public abstract class ApplicationBase
{
    public IMapper Mapper { get; set; }
    public IUserRepository UserRepository { get; set; }

    public ApplicationBase(IUserRepository userRepository, IMapper mapper)
    {
        UserRepository = userRepository;
        Mapper = mapper;
    }
}
