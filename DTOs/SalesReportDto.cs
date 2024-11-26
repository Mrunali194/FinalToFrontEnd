public class SalesReportDto
{
    public int OrderId {get; set;}
    public int AmountSold {get; set;}
    public decimal SaleAmount {get; set;}

    public OrderDetails? Order { get; set; } 
}