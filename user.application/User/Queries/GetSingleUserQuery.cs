namespace user.application.User.Queries;

public class GetSingleUserQuery : IRequest<UserDTO>
{
    public string Email { get; set; }
    public class GetSingleUserHandler : ApplicationBase, IRequestHandler<GetSingleUserQuery, UserDTO>
    {
        public GetSingleUserHandler(IUserRepository userRepository, IMapper mapper)
            : base(userRepository: userRepository, mapper: mapper) { }

        public async Task<UserDTO> Handle(GetSingleUserQuery request, CancellationToken cancellationToken)
        {
            UserGetSingleSpecification specification = new(request.Email);
            var data = await this.UserRepository.GetItemsAsync(specification);
            var res = Mapper.Map(data.FirstOrDefault(), new UserDTO());
            return await Task.FromResult(res);
        }
    }
}