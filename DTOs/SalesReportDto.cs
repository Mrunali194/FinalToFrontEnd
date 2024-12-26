using System.ComponentModel.DataAnnotations;

namespace Demo.Dtos;
public class SalesReportDto
{ 
    
        public int DrugId { get; set; } 
        public string DrugName { get; set; }  // Name of the drug/product (optional, if needed)
        public int AmountSold { get; set; }  // Total quantity sold for this product
        public decimal SaleAmount { get; set; }
}