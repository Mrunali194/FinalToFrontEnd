public class OrderCancelDto
{
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public string OrderStatus { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string TransactionStatus { get; set; }
}
