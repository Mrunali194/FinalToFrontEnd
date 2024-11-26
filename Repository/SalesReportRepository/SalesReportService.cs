// SalesReportRepository.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.Database;
using Demo.Dtos;
using Demo.Model;
using Microsoft.EntityFrameworkCore;

namespace Demo.Repository
{
    public class SalesReportService : ISalesReportRepository
    {
        private readonly DatabaseContext context;

        public SalesReportService(DatabaseContext _context)
        {
            context = _context;
        }

        // public async Task<List<SalesReportDto>> GetSalesReportByProduct()
        // {
            
        //     var reportData = await context.OrderItems
        //         .GroupBy(oi => oi.DrugId)  // Group by DrugId (product)
        //         .Select(group => new SalesReport
        //         {
        //             DrugId = group.Key,  
        //             Drug = group.FirstOrDefault().Drug,  
        //             AmountSold = group.Sum(oi => oi.Quantity),  
        //             SaleAmount = group.Sum(oi => oi.Quantity * oi.Price)  
        //         })
        //         .ToListAsync();

           
        //     context.SalesReports.AddRange(reportData);  
        //     await context.SaveChangesAsync();  
            
        //     var reportDtos = reportData.Select(report => new SalesReportDto
        //     {
        //         DrugId = report.DrugId,
        //         DrugName = report.Drug.DrugName,
        //         AmountSold = report.AmountSold,
        //         SaleAmount = report.SaleAmount
        //     }).ToList();

        //     return reportDtos;  
        // }

        public async Task<List<SalesReportDto>> GetSalesReportByProduct()
        {
           
            var reportData = await context.OrderItems
                .GroupBy(oi => oi.DrugId)  
                .Select(group => new SalesReport
                {
                    DrugId = group.Key,  
                    Drug = group.FirstOrDefault().Drug,  
                    AmountSold = group.Sum(oi => oi.Quantity),  
                    SaleAmount = group.Sum(oi => oi.Quantity * oi.Price)  
                })
                .ToListAsync();

            
            foreach (var report in reportData)
            {
               
                var existingReport = await context.SalesReports
                    .FirstOrDefaultAsync(sr => sr.DrugId == report.DrugId);

                if (existingReport != null)
                {
                    existingReport.AmountSold = report.AmountSold;
                    existingReport.SaleAmount = report.SaleAmount;
                    
                }
                else
                {
                    
                    context.SalesReports.Add(report);
                }
            }

           
            await context.SaveChangesAsync();

            
            var reportDtos = reportData.Select(report => new SalesReportDto
            {
                DrugId = report.DrugId,
                DrugName = report.Drug.DrugName,
                AmountSold = report.AmountSold,
                SaleAmount = report.SaleAmount
            }).ToList();

            return reportDtos;  

            }
    }
}
