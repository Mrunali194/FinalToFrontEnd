
using System.ComponentModel.DataAnnotations;

public class InventoryDetails {
    [Key]
    public int DrugId {get; set;}

    
    public string? DrugName {get; set;}

    
    public int Quantity {get; set;}

   
    public DateTime ExpiryDate {get; set;}

    
    public decimal Price {get; set;}

    
    public int SupplierId {get; set;}
    public SupplierDetails? Supplier { get; set; }//one inventory has only one supplier


     public ICollection<DrugsCart>? DrugCarts { get; set; }
     //Many-to-Many: A user can have many drugs in their cart, and a drug can be in many users' carts. This means multiple users can add the same drug to their cart, and each user can have multiple drugs in their cart.

      public ICollection<OrderDetails>? OrderDetails{get;set;} 
}