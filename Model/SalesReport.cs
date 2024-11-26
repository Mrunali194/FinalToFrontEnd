
using System.ComponentModel.DataAnnotations;
namespace Demo.Model;
public class SalesReport {
    [Key]
    public int ReportId {get; set;}
    public int OrderId {get; set;}
    public int AmountSold {get; set;}
    public decimal SaleAmount {get; set;}

    public OrderDetails? Order { get; set; } 
}