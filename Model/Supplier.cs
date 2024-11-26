using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace Demo.Model;
public class SupplierDetails {
    [Key]
    public int? SupplierId {get; set;}

    [Required]
    public string? SupplierEmail {get; set;}

    [Required]
    public string? SupplierName {get; set;}

    [Required]
    public string? Contact {get; set;}

    //  is onetomany mapping
    //one supplier can supply multiple drugs
    
    public ICollection<DrugDetails> Drugs { get; set; }
}