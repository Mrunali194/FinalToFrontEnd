using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Demo.Dtos;
public class UserDetailsDto
{
    public string? UserName {get; set;}

    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string? EmailId {get; set;}

    [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile number must be exactly 10 digits.")]
    [Phone(ErrorMessage = "Please enter a valid phone number.")]
    public string? Contact {get; set;}

    [RegularExpression(@"^[A-Z][A-Za-z\d!@#$%^&*]{7,11}$", ErrorMessage ="Password must be strong")]
    public string? Password {get; set;}
   
   [JsonIgnore]
    public string? UserType {get; set;}
}