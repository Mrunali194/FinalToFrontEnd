using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.Model;

public class UserDetails {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserId {get; set;}
    public string? UserName {get; set;}
    public string EmailId {get; set;}
    public string? Contact {get; set;}
    public string? Password {get; set;}
    public string UserType {get; set;}


    //public List<OrderDetails>? Orders{get;set;}
    
}