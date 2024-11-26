
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace Demo.Model;
public class DrugDetails {
    [Key]
    public int DrugId {get; set;}
    public string? DrugName {get; set;}
    public int Quantity {get; set;}
    public DateTime ExpiryDate {get; set;}
    public decimal Price {get; set;}

   
    public int? SupplierId {get; set;}

    [JsonIgnore]
    public SupplierDetails? Supplier { get; set; }//one inventory has only one supplier

    [JsonIgnore]
    public ICollection<CartItem>? CartItems { get; set; }

    
}