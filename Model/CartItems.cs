using System.ComponentModel.DataAnnotations;
using Demo.Model;

public class CartItem
{
    [Key]
    public int CartItemId { get; set; }
    public int CartId { get; set; }  // Foreign key to Cart
    public DrugsCart Cart { get; set; }   // Navigation property for the related cart
    
    public int DrugId { get; set; }  // Foreign key to Drug
    public DrugDetails Drug { get; set; }   // Navigation property for the related drug
    
    public int Quantity { get; set; }
    public decimal Price { get; set; }

    // public int? OrderId { get; set; }  // Nullable, as the item might not have been ordered yet
    
    // // Navigation property to the related Order
    // public OrderDetails? Order { get; set; } 
}
