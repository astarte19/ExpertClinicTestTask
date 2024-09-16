namespace ExpertClinicTestTask.Domain.Core.Dtos.Account;

public class UserCreationDto
{
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string Patronymic { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string PassportNumber { get; set; }
    public string PlaceOfBirth { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string RegistrationAddress { get; set; }
    public string LivingAddress { get; set; }
}