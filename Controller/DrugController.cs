using Demo.Dtos;
using Demo.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

[Route("/api/[controller]")]
public class DrugController : ControllerBase
{

    private readonly IDrugRepository drugDetails;
    public DrugController(IDrugRepository _drugDetails )
     {
        drugDetails= _drugDetails;
    }

    [HttpPost("add")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddDrug([FromBody] DrugDetailsDto drugDto)
    {
        
        if (drugDto.ExpiryDate <= DateTime.Now)
        {
            return BadRequest("Expiry date must be in the future.");
        }

        if (drugDto == null)
        {
            return BadRequest("Drug details are required.");
        }
        try
        {
            
            await drugDetails.AddDrugDetails(drugDto);

            
            return Ok(new { Message = "Drug added successfully." });
        }
        catch (Exception ex)
        {
            
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("deleteExpireDrug")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteExpiredDrugs()
    {
        try
        {
            var expiredDrugs = await drugDetails.GetDrugsToUpdateAsync();

            if (expiredDrugs == null || expiredDrugs.Count == 0)
            {
                return NotFound(new { message = "No expired drugs found" });
            }

            await drugDetails.DeleteDrugsAsync(expiredDrugs);

            return Ok(new { message = $"{expiredDrugs.Count} having expired drugs deleted successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

         [Authorize(Roles = "Doctor,Admin")]
        [HttpGet("searchByName/{searchDrug}")]
        public async Task<IActionResult> SearchDrugsByName(string searchDrug)
        {
            if (string.IsNullOrWhiteSpace(searchDrug))
            {
                return BadRequest(new { message = "Drug Name cannot be empty." });
            }

            var drugs = await drugDetails.GetAllDrugs()
                                           .Where(d => EF.Functions.Like(d.DrugName, $"{searchDrug}%"))
                                           .ToListAsync();

            if (drugs == null || drugs.Count == 0)
            {
                return NotFound(new { message = "No drugs found with the specified name." });
            }

            return Ok(drugs); 
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update/{drugId}")]
        public async Task<IActionResult> UpdateDrugQuantity(int drugId, [FromBody] DrugDetailsDto drugDto)
        {
            
            var drug = await drugDetails.GetDrugByIdAsync(drugId);

            if (drug == null)
            {
                return NotFound(new { message = "Drug not found." });
            }
           
            drug.Quantity -= drugDto.Quantity; 
            if (drug.Quantity < 0)
            {
                return BadRequest(new { message = "Insufficient stock." });
            }

            try
            {
                await drugDetails.UpdateDrugDetails(drug);
                return Ok(new { message = "Drug quantity updated successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    [HttpGet]
    [Authorize(Roles ="Doctor,Admin")]
    public async Task<IActionResult> GetAllDrugs()
    {
        var drugs = await drugDetails.GetAllDrugs().ToListAsync();
        return Ok(drugs);
    }

    
}