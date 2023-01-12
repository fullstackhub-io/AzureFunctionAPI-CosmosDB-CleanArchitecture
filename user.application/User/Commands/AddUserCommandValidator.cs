namespace user.application.User.Commands;

public class AddUserCommandValidator : AbstractValidator<AddUserCommand>
{
    public AddUserCommandValidator()
    {
        this.RuleFor(v => v.FirstName).NotEmpty().WithMessage("Enter first name");
        this.RuleFor(v => v.LastName).NotEmpty().WithMessage("Enter last name");
        this.RuleFor(v => v.DateOfBirth).NotEmpty().WithMessage("Enter date of birth");
        this.RuleFor(v => v.Gender).Must(x => (new string[] { "M", "F", "m", "f" }).Contains(x)).WithMessage("Select gender");
        this.RuleFor(v => v.Email).NotEmpty().WithMessage("Enter email");
        this.RuleFor(v => v.PhoneNumber).NotEmpty().WithMessage("Enter phone #");
    }
}
