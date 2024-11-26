// ISalesReportRepository.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Dtos;

namespace Demo.Repository
{
    public interface ISalesReportRepository
    {
        Task<List<SalesReportDto>> GetSalesReportByProduct();
    }
}
