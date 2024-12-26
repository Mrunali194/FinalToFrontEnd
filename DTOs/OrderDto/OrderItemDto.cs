namespace Demo.Dtos;
public class OrderItemDto
{
    public int OrderItemId { get; set; }  
    public int DrugId { get; set; }  
    public string DrugName { get; set; }  
    public int Quantity { get; set; }  
    public decimal Price { get; set; }  
    public decimal TotalItemPrice { get; set; } 
}
