using System.Text.Json.Serialization;

namespace Demo.Dtos;
public class UserDetailsDto
{
    public string? UserName {get; set;}
    public string? EmailId {get; set;}
    public string? Contact {get; set;}
    public string? Password {get; set;}
   
   [JsonIgnore]
    public string? UserType {get; set;}
}