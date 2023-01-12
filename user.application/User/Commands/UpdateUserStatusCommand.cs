namespace user.application.User.Commands;

public class UpdateUserStatusCommand : IRequest<bool>
{
    public string? ID { get; set; }
    public UserStatus Status { get; set; }

    public class UpdateUserStatusCommandHandler : ApplicationBase, IRequestHandler<UpdateUserStatusCommand, bool>
    {
        public UpdateUserStatusCommandHandler(IUserRepository userRepository, IMapper mapper)
            : base(userRepository: userRepository, mapper: mapper) { }

        public async Task<bool> Handle(UpdateUserStatusCommand request, CancellationToken cancellationToken)
        {
            var entity = await this.UserRepository.GetItemAsync(request.ID);
            if (entity == null)
            {
                throw new EntityNotFoundException(nameof(domain.Entities.User), request.ID);
            }

            entity.Status = request.Status;

            await this.UserRepository.UpdateItemAsync(request.ID, entity);

            return true;
        }
    }
}