using System.ComponentModel.DataAnnotations;
using Demo.Model;

public class OrderItem
{
    [Key]
    
    public int OrderItemId{get;set;}
    // Foreign Key to Order
    public int OrderId { get; set; }
    public OrderDetails Order { get; set; }  // Navigation property to Order

    // Foreign Key to Drug (or Product) table
    public int DrugId { get; set; }
    public DrugDetails Drug { get; set; }  // Navigation property to Drug (or Product)

    public int Quantity { get; set; }  // Quantity of the drug ordered

    public decimal Price { get; set; }  
}
