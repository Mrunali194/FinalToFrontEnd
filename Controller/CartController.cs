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

   
    [HttpPost("confirm")]
    [Authorize(Roles ="Doctor")]
    public async Task<IActionResult> ConfirmCart()
    {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("Invalid User ID.");
            }

            
            var cart = await _context.DrugsCarts
                                    .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)   
            {
                return BadRequest("No confirmed cart found or cart has already been placed.");
            }

            
            // if (cart.isConfirmed)
            // {
            //     return BadRequest("This cart has already been confirmed.");
            // }

            
            var cartItems = await _context.CartItems
                                        .Where(ci => ci.CartId == cart.CartId)
                                        .ToListAsync();

    
            if (!cartItems.Any())
            {
                return BadRequest("Your cart is empty.");
            }

            // Deduct stock for each cart item only when confirming the order
            foreach (var cartItem in cartItems)
            {
                var drug = await _context.DrugDetails.FindAsync(cartItem.DrugId);
                if (drug == null)
                {
                    return NotFound($"Drug with ID {cartItem.DrugId} not found.");
                }

                // Ensure enough stock is available before confirming the cart
                if (drug.Quantity < cartItem.Quantity)
                {
                    return BadRequest($"Not enough stock for {drug.DrugName}");
                }

                
                // drug.Quantity -= cartItem.Quantity;
            }

            
            cart.TotalPrice = cartItems.Sum(ci => ci.Quantity * ci.Price);
            cart.Quantity = cartItems.Sum(ci => ci.Quantity);

            // Mark the cart as confirmed 
            //cart.isConfirmed = true;  

     
            await _context.SaveChangesAsync();

            return Ok("Cart is confirmed");
    }
}
