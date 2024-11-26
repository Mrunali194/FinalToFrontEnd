using System.ComponentModel.DataAnnotations;

public class TransactionDetails
{
    [Key]
    public int TransactionId{get;set;}
    public string? TransactionType{get;set;}
    public string? TransactionStatus{get;set;}
    public decimal AmountPaid{get;set;}
    public DateTime TransactionDate{get;set;}
    public int CartId{get;set;}
    public int OrderId{get;set;}

    public DrugsCart? Cart{get;set;}
    public OrderDetails? Order{get;set;}

}
    
       


