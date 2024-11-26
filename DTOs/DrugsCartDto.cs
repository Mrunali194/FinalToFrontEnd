public class DrugsCartDto{
    public int DrugId {get; set;}
    public int UserId {get; set;}
    public int Quantity {get; set;}
    public decimal TotalPrice {get; set;}

    public UserDetails? User { get; set; }       // Navigation property to User
    public InventoryDetails? Drug { get; set; }

    public TransactionDetails? Transaction{get;set;}
}