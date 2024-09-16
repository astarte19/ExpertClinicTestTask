using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ExpertClinicTestTask.Domain.Core.Account;

public class User
{
    [Key]
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [Required]
    [JsonPropertyName("lastName")]
    public string LastName { get; set; }

    [Required]
    [JsonPropertyName("firstName")]
    public string FirstName { get; set; }

    [JsonPropertyName("patronymic")]
    public string Patronymic { get; set; }

    [Required]
    [JsonPropertyName("dateOfBirth")]
    public DateTime DateOfBirth { get; set; }

    [Required]
    [JsonPropertyName("passportNumber")]
    [MaxLength(14)]
    [RegularExpression(@"^\d{4}\s\d{6}$")]
    public string PassportNumber { get; set; }

    [Required]
    [JsonPropertyName("placeOfBirth")]
    public string PlaceOfBirth { get; set; }

    [Required]
    [JsonPropertyName("phoneNumber")]
    [MaxLength(11)]
    [RegularExpression(@"^7\d{10}$")]
    public string PhoneNumber { get; set; }

    [Required]
    [JsonPropertyName("email")]
    [EmailAddress]
    public string Email { get; set; }

    [JsonPropertyName("registrationAddress")]
    public string RegistrationAddress { get; set; }

    [JsonPropertyName("livingAddress")]
    public string LivingAddress { get; set; }

}