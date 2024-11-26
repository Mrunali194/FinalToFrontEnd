
using System.ComponentModel.DataAnnotations;
namespace Demo.Model;
public class SalesReport {
        [Key]
        public int ReportId { get; set; }  // Unique identifier for each product-level report

        public int DrugId { get; set; }  // The ID of the drug/product that was sold

        public int AmountSold { get; set; }  // The total quantity of the drug sold

        public decimal SaleAmount { get; set; }  // The total sales amount for the drug (quantity * price)

        // Navigation property to the Drug table
        public DrugDetails Drug { get; set; }  
}