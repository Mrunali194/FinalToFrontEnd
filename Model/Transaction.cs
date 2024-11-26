using System.ComponentModel.DataAnnotations;
namespace Demo.Model;
public class TransactionDetails
{
    [Key]
    public int TransactionId{get;set;}
    public string? TransactionType{get;set;}
    public string? TransactionStatus{get;set;}
    public decimal AmountPaid{get;set;}
    public DateTime? TransactionDate{get;set;}

    public int OrderId{get;set;}
    public OrderDetails? Order{get;set;}

}
    
       


