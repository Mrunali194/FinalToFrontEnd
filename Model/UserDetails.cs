using System.ComponentModel.DataAnnotations;

public class UserDetails {
    [Key]
    public int UserId {get; set;}
    public String? UserName {get; set;}
    public String? EmailId {get; set;}
    public String? Contact {get; set;}
    public String? Password {get; set;}
    public String? UserType {get; set;}

    public ICollection<DrugsCart>? DrugCarts { get; set; }
    public ICollection<OrderDetails>? OrderDetails{get;set;}
    

}