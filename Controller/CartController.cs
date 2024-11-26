using Demo.Database;
using Demo.Dtos;
using Demo.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]  
public class CartController : ControllerBase
{
    private readonly IAddTocart addToCart;
    private readonly DatabaseContext _context;  

    
    public CartController(IAddTocart _addToCart, DatabaseContext context)
    {
        addToCart = _addToCart;
        _context = context;
    }

   
    [HttpPost("addToCart")]
    [Authorize(Roles = "Doctor")]  
    public async Task<IActionResult> AddToCart([FromBody] AddToCartDto addToCartDto)
    {
        if (addToCartDto == null || addToCartDto.Quantity <= 0)
        {
            return BadRequest("Invalid drug data.");
        }

        try
        {
            var result = await addToCart.AddToCart(addToCartDto, User);  
            return Ok(result);  
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);  
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);  
        }
    }

   
}
