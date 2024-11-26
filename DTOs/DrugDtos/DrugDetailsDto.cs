using System.Text.Json.Serialization;

namespace Demo.Dtos;
public class DrugDetailsDto
{
    public string? DrugName {get; set;}
    public int Quantity {get; set;}
     public DateTime ExpiryDate { get; set; }
    public decimal Price {get; set;}
    
    public int SupplierId{get;set;}
    
}