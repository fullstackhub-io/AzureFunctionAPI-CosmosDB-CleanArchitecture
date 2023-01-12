namespace user.application.User.Commands;

public class UpdateUserCommand : IRequest<bool>
{
    public string? ID { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Zip { get; set; }
    public string? Country { get; set; }

    public class UpdateUserCommandHandler : ApplicationBase, IRequestHandler<UpdateUserCommand, bool>
    {
        public UpdateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
            : base(userRepository: userRepository, mapper: mapper) { }

        public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var entity = await this.UserRepository.GetItemAsync(request.ID);
            if (entity == null)
            {
                throw new EntityNotFoundException(nameof(domain.Entities.User), request.ID);
            }

            entity.PhoneNumber = request.PhoneNumber;
            entity.Address1 = request.Address1;
            entity.Address2 = request.Address2;
            entity.City = request.City;
            entity.State = request.State;
            entity.Zip = request.Zip;
            entity.Country = request.Country;

            await this.UserRepository.UpdateItemAsync(request.ID, entity);

            return true;
        }
    }
}

