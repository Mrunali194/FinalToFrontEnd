
using System.Security.Claims;
using Demo.Database;
using Demo.Dtos;
using Demo.Model;
using Demo.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

[ApiController]
[Route("/api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderRepository orderService;
    
    public OrderController(IOrderRepository _orderService)
    {
        orderService = _orderService;
    }

    [HttpPost("place-order")]
    [Authorize(Roles ="Doctor")]
    public async Task<IActionResult> PlaceOrder()
    {
        try
        {
            var result = await orderService.PlaceOrder(User.Identity as ClaimsIdentity);
            return Ok(result); // Return a success message
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message }); // Return a bad request with the error message
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
          
    }

    [HttpGet("all-orders")]
    [Authorize(Roles = "Admin")]  // Only Admin can access all orders
    public async Task<IActionResult> GetAllOrders()
    {
        try
        {
            var orders = await orderService.GetAllOrdersWithDrugs();
            
            if (orders == null || !orders.Any())
            {
                return NotFound(new { message = "No orders found." });
            }

            return Ok(orders);
        }
        catch (Exception)
        {
            throw new Exception("Internal server error: {ex.Message}");
        }
    }

    [Authorize(Roles = "Admin")]  // Restrict to admin users
    [HttpPut("verify-order/{orderId}")]
    public async Task<IActionResult> VerifyOrder(int orderId)
    {
        try
        {
            var result = await orderService.VerifyOrder(orderId);
            return Ok(result);  // Return the result of the verification
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}


