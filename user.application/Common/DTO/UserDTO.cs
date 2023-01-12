namespace user.application.Common.DTO;

public class UserDTO : IMapFrom<domain.Entities.User>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Password { get; set; }
    public string? Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public UserStatus? Status { get; set; }
    public UserType? UserType { get; set; }
    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Zip { get; set; }
    public string? Country { get; set; }

    public string? Salutation { get; set; }
    public int? Age { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<domain.Entities.User, UserDTO>()
             .ForMember(d => d.Salutation, opt => opt.MapFrom(s => s.Gender.ToUpper() == "M" ? "Hi Sir!" : "Hi Ma'am!"))
             .ForMember(d => d.Age, opt => opt.MapFrom(s => DateTime.Today.Year - s.DateOfBirth.Value.Year));
        profile.CreateMap<UserDTO, domain.Entities.User>();
    }
}
