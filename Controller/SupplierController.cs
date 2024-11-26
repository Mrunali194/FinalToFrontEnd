
using Demo.Model;
using Microsoft.AspNetCore.Mvc;
using Demo.Repository;
using Demo.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

[ApiController]

[Route("/api/[controller]")]
public class SupplierController : ControllerBase
{

    private readonly ISupplierRepository supplierDetails;
    public SupplierController(ISupplierRepository _supplerDetails )
     {
        supplierDetails= _supplerDetails;
    }

    [HttpPost("Add")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddSupplier([FromBody] SupplierDetailsDto supplierDto)
    {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try{

            
                var result = supplierDetails.AddSupplierDetails(supplierDto);
                return Ok(new 
                {
                    message = "Supplier Added successfully",
                    user = result
                });
            }
            catch(Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
                
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("getAllSupplier")]
    public async Task<ActionResult<List<SupplierDetails>>> GetAllSuppliers()
    {
        var suppliers = await supplierDetails.GetAllSuppliers();

        if (suppliers == null || !suppliers.Any())
        {
            return NotFound("No suppliers found.");
        }

        return Ok(suppliers);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("searchByName/{supplierName}")]
    public async Task<IActionResult> SearchSuppliersByName(string supplierName)
    {
        if (string.IsNullOrWhiteSpace(supplierName))
        {
            return BadRequest(new { message = "Supplier name cannot be empty." });
        }

        
        var suppliers = await supplierDetails.GetAllSuppliers();

        var filteredSuppliers =  suppliers.Where(s => s.SupplierName.ToLower().StartsWith(supplierName.ToLower())) 
                                                .ToList();

        if (filteredSuppliers == null || filteredSuppliers.Count == 0)
        {
            return NotFound(new { message = "No suppliers found with the specified name." });
        }

        return Ok(filteredSuppliers);
    }


    [Authorize(Roles = "Admin")]
    [HttpDelete("delete/{supplierId}")]
    public async Task<IActionResult> DeleteSupplier(int supplierId)
    {
        try
        {
            var supplier = await supplierDetails.GetSupplierByIdAsync(supplierId);
            if (supplier == null)
            {
                return NotFound(new { message = "Supplier not found." });
            }
            await supplierDetails.DeleteSupplierAsync(supplier);

            return Ok(new { message = "Supplier deleted successfully." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

}


