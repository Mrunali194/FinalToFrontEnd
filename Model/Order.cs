using System.ComponentModel.DataAnnotations;
namespace Demo.Model;
public class OrderDetails {
    [Key]
    public int OrderId {get; set;}
    
    public int UserId {get; set;}
    public DateTime OrderDate {get; set;}
    public string? OrderStatus {get; set;}
    public int OrderQuantity {get; set;}

    public UserDetails User{get;set;}
    
    
    //public ICollection<CartItem> CartItems { get; set; } 
    public ICollection<OrderItem> OrderItems { get; set; }

   
    
}