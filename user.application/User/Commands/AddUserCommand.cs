namespace user.application.User.Commands;

public class AddUserCommand : IRequest<string>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public UserType? UserType { get; set; }
    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Zip { get; set; }
    public string? Country { get; set; }

    public class AddNewUserHandler : ApplicationBase, IRequestHandler<AddUserCommand, string>
    {
        public AddNewUserHandler(IUserRepository userRepository, IMapper mapper)
            : base(userRepository: userRepository, mapper: mapper) { }

        public async Task<string> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            domain.Entities.User entity = new()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Gender = request.Gender,
                DateOfBirth = request.DateOfBirth,
                Status = UserStatus.PENDING,
                UserType = request.UserType.ToString(),
                Address1 = request.Address1,
                Address2 = request.Address2,
                City = request.City,
                State = request.State,
                Zip = request.Zip,
                Country = request.Country
            };

            await this.UserRepository.AddItemAsync(entity);


            return entity.Id;
        }
    }
}
