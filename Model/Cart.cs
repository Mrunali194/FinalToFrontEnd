using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Eventing.Reader;
namespace Demo.Model;
public class DrugsCart {
    [Key]
    public int CartId {get; set;}
    public int UserId {get; set;}
    public int Quantity {get; set;}
    public decimal TotalPrice {get; set;}

    
    //public bool isConfirmed{get;set;}
    public UserDetails? User { get; set; }       // Navigation property to User

    //public TransactionDetails? Transaction{get;set;}
    public ICollection<CartItem>? CartItems{get;set;}

}