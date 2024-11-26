
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
    public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderDto placeOrderDto)
    {
        try
        {
            var idString = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            if (string.IsNullOrEmpty(idString))
            {
                return BadRequest("UserId claim is missing.");
            }

            // Attempt to parse the UserId claim to an integer
            if (!int.TryParse(idString, out int id))
            {
                return BadRequest("Invalid UserId.");
            };
            
            var result = await orderService.PlaceOrder(id,placeOrderDto);
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
    [HttpGet("orders")]
    [Authorize(Roles ="Doctor")]
    public async Task<IActionResult> GetUserOrders()
    {
        try
        {
            var userId =int.Parse(User.FindFirst("UserId")?.Value);

            var orders = await orderService.GetUserOrders(userId);

            if (orders == null || orders.Count == 0)
            {
                return NotFound("No orders found for the user.");
            }

            return Ok(orders);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error: {ex.Message}");
        }
    }
    [HttpPost("approve-order/{orderId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ApproveOrder(int orderId)
    {
        
        var result = await orderService.ApproveOrder(orderId);

        if (result.Contains("successfully"))
        {
            return Ok(result);  
        }
        else
        {
            return BadRequest(result);  
        }
    }

    [HttpPost("cancel")]
    [Authorize(Roles ="Doctor")]
    public async Task<IActionResult> CancelCompletedOrder(int OrderId)
    {
            try
            {
                var userid = int.Parse(User.FindFirst("UserId")?.Value);  
                var result = await orderService.CancelCompletedOrder(OrderId,userid);

                return Ok(new { message = result });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
}


