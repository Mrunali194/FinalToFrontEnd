public class SupplierDetailsDto
{
    public String? SupplierEmail {get; set;}
    public String? SupplierName {get; set;}
    public String? Contact {get; set;}
    //  is onetomany mapping
    //one supplier has many inventories
    public ICollection<InventoryDetails>? Inventories { get; set; }
}