using System.ComponentModel.DataAnnotations;

public class SupplierDetails {
    [Key]
    public int SupplierId {get; set;}

    [Required]
    public String? SupplierEmail {get; set;}

    [Required]
    public String? SupplierName {get; set;}

    [Required]
    public String? Contact {get; set;}

    //  is onetomany mapping
    //one supplier has many inventories
    public ICollection<InventoryDetails>? Inventories { get; set; }

}