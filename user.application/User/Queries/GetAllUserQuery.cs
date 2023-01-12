namespace user.application.User.Queries;

public class GetAllUserQuery : IRequest<UserVM>
{
    public UserStatus Status { get; set; }
    public class GetAllUserHandler : ApplicationBase, IRequestHandler<GetAllUserQuery, UserVM>
    {
        public GetAllUserHandler(IUserRepository userRepository, IMapper mapper)
            : base(userRepository: userRepository, mapper: mapper) { }

        public async Task<UserVM> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            UserGetAllSpecification specification = new(request.Status);
            var data = await this.UserRepository.GetItemsAsync(specification);
            var res = Mapper.Map(data, new List<UserDTO>());

            return await Task.FromResult(new UserVM() { UserList = res });
        }
    }
}