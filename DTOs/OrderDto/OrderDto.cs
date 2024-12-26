namespace Demo.Dtos;
public class AllOrderDetails
{
    public int OrderId { get; set; }  
    public int UserId { get; set; }  
    public DateTime OrderDate { get; set; }  
    public string OrderStatus { get; set; }  
    public int OrderQuantity { get; set; }  

    public string TransactionStatus{get;set;}

    // A list of OrderItems, represented by OrderItemDto
    public List<OrderItemDto> OrderItems { get; set; }  
}