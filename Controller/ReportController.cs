
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Database;
using Demo.Dtos;
using Demo.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesReportController : ControllerBase
    {
        private readonly ISalesReportRepository salesReportService;
        private readonly DatabaseContext context;

        public SalesReportController(ISalesReportRepository _salesReportService,DatabaseContext _context)
        {
            salesReportService = _salesReportService;
            context=_context;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<List<SalesReportDto>> GetSalesReportByProduct()
        {
            return await salesReportService.GetSalesReportByProduct();
        }
    }
}
