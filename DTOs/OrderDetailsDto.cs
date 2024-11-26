public class OrderDetailsDto
{
    public int UserId {get; set;}
    public int DrugId {get; set;}
    public DateTime OrderDate {get; set;}
    public string? OrderStatus {get; set;}
    public int OrderQuantity {get; set;}

    public UserDetails? User{get;set;}
    //public InventoryDetails? Drugs{get;set;}
    public ICollection<InventoryDetails>? Inventories{get;set;}

    public SalesReport? SalesReport { get; set; }

    public TransactionDetails? Transaction{get;set;}
}