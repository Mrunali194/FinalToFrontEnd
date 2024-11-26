using Demo.Database;
using Demo.Dtos;
using Demo.Model;
using Demo.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Demo.Repository;

    public class OrderService : IOrderRepository
    {
        private readonly DatabaseContext context;
        private readonly IMailService mailService;

    public OrderService(DatabaseContext _context,IMailService _mailService)
    {
            context = _context;
            mailService=_mailService;
    }
    
    private string GenerateOrderConfirmationEmail(OrderDetails order, List<OrderItem> orderItems)
    {
        var orderItemsHtml = string.Join("", orderItems.Select(item =>
            $"<tr><td>{item.Drug.DrugName}</td><td>{item.Quantity}</td><td>{item.Price:C}</td><td>{item.Price*item.Quantity:C}</td></tr>"
        ));

        var emailBody = $@"
            <html>
            <body>
                <h2>Order Confirmation</h2>
                <p>Dear Customer,</p>
                <p>Your order has been placed successfully!</p>
                <p><strong>Order ID:</strong> {order.OrderId}</p>
                <p><strong>Order Date:</strong> {order.OrderDate:yyyy-MM-dd HH:mm:ss}</p>
                <p><strong>Status:</strong> {order.OrderStatus}</p>
                <h3>Order Items:</h3>
                <table border='1'>
                    <tr>
                        <th>Drug</th>
                        <th>Quantity</th>
                        <th>Price</th>
                        <th>Total</th>
                    </tr>
                    {orderItemsHtml}
                </table>
                <p>Thank you for shopping with us! We will notify you once your order is verified.</p>
                <p>Best regards,<br/>Pharmacy system</p>
            </body>
            </html>";

        return emailBody;
    } 
    
    public async Task<string> PlaceOrder(ClaimsIdentity user)
    {
            var userId = int.Parse(user.FindFirst("UserId")?.Value);

            // // Retrieve the user's cart
            // var cart = await context.DrugsCarts
            //     .Include(c => c.CartItems)  // Include the related CartItems
            //     .ThenInclude(ci => ci.Drug) // Include the related Drugs
            //     .FirstOrDefaultAsync(c => c.UserId == userId);  // Get the user's cart

            var cart = await context.DrugsCarts
                        .Include(c => c.CartItems)  // Include related CartItems
                        .ThenInclude(ci => ci.Drug) // Include the related Drugs (DrugDetails)
                        .FirstOrDefaultAsync(c => c.UserId == userId);  // Get the user's cart

            if (cart == null || cart.CartItems.Count == 0)
            {
                throw new InvalidOperationException("Your cart is empty.");
            }

            // Create a new OrderDetails object
            var order = new OrderDetails
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                OrderStatus = "Pending", // Initial status can be 'Pending'
                OrderQuantity = cart.CartItems.Sum(ci => ci.Quantity)
                 // Total quantity of drugs in the order
            };

            // Add the order to the database first to generate the OrderId
            await context.OrderDetails.AddAsync(order);
            await context.SaveChangesAsync();  // Save OrderDetails to get the OrderId

           
            // Create a list to store the OrderItems
            var orderItems = new List<OrderItem>();

            foreach (var cartItem in cart.CartItems)
            {
                var drug = cartItem.Drug;
                if (drug == null)
                {
                    throw new KeyNotFoundException($"Drug with ID {cartItem.DrugId} not found.");
                }
                var orderItem = new OrderItem
                {
                    OrderId = order.OrderId,  // Now that OrderId is generated, assign it to the OrderItem
                    DrugId = cartItem.DrugId,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Price // Save the price at the time of the order
                };

                orderItems.Add(orderItem);

                // Decrease the stock based on the quantity ordered (optional)
                // drug.Quantity -= cartItem.Quantity;
            }

            // Add the order items to the database
            await context.OrderItems.AddRangeAsync(orderItems);

            // Now, save the changes to persist the OrderItems in the database
            await context.SaveChangesAsync();

            // Now, delete the cart items since they have been placed into the order
            context.CartItems.RemoveRange(cart.CartItems);
            await context.SaveChangesAsync();

            // Optionally, delete the Cart itself, if you no longer need it
            context.DrugsCarts.Remove(cart);
            await context.SaveChangesAsync();

            var emailBody = GenerateOrderConfirmationEmail(order, orderItems);
            var userEmail = user.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(userEmail))
            {
                throw new InvalidOperationException("User email not found.");
            }
            // Create the MailRequestDTO object
            var mailRequest = new MailRequestDTO
            {
                ToEmail = userEmail,  // The recipient's email address
                Subject = "Order Confirmation",  // The subject of the email
                Body = emailBody  // The body content of the email (HTML)
            };
             mailService.SendEmail(mailRequest);

            return "Order placed Successfully";  // Return the placed order
        }

        public async Task<List<OrderWithDrugsDto>> GetAllOrdersWithDrugs()
        {

            try
            {
                var orders = await context.OrderDetails
                    .Include(o => o.OrderItems)  // Include related OrderItems
                    .ThenInclude(oi => oi.Drug)  // Include the Drug details in the OrderItem
                    .Where(o => o.OrderStatus != "Verified")  // Show only unverified orders
                    .ToListAsync();

                var orderDtos = orders.Select(o => new OrderWithDrugsDto
                {
                    OrderId = o.OrderId,
                    UserId = o.UserId,
                    OrderDate = o.OrderDate,
                    OrderStatus = o.OrderStatus,
                    OrderQuantity = o.OrderQuantity,
                    OrderItems = o.OrderItems.Select(oi => new OrderItemDto
                    {
                        OrderItemId=oi.OrderItemId,
                        DrugId = oi.DrugId,
                        DrugName = oi.Drug.DrugName,  // Assuming Drug has a Name property
                        Quantity = oi.Quantity,
                        TotalItemPrice = oi.Quantity * oi.Price,
                        Price = oi.Price
                    }).ToList()
                }).ToList();

                return orderDtos;
            }
            catch (Exception ex)
            {
                 throw new Exception($"Error fetching orders: {ex.Message}");
            }
        }
public async Task<string> VerifyOrder(int orderId)
{
    // var order = await context.OrderDetails
    // .Include(o => o.User)  // Explicitly include the User in the query
    // .FirstOrDefaultAsync(o => o.OrderId == orderId);

    // Retrieve the order details along with the order items
        var order = await context.OrderDetails
        .Include(o => o.User)  // Load related User
        .Include(o => o.OrderItems)  // Load related OrderItems
        .ThenInclude(oi => oi.Drug)  // Load related DrugDetails for each OrderItem
        .FirstOrDefaultAsync(o => o.OrderId == orderId);
    // Check if the order is null (if the order doesn't exist in the database)
    if (order == null)
    {
        throw new InvalidOperationException("Order not found.");
    }

    // Check if the User is null (ensure the order is associated with a valid user)
    if (order.User == null)
    {
        throw new InvalidOperationException("Order does not have an associated user.");
    }

    bool isStockAvailable = true;
    List<OrderItem> outOfStockItems = new List<OrderItem>();

    // Check stock availability for each item in the order
    foreach (var orderItem in order.OrderItems)
    {
        // Check if the Drug is null (ensure the order item has a valid drug associated)
        if (orderItem.Drug == null)
        {
            throw new InvalidOperationException($"Drug not found for OrderItem ID {orderItem.OrderItemId}");
        }

        var drug = orderItem.Drug;

        // If the drug quantity in stock is less than the quantity ordered, mark it as out of stock
        if (drug.Quantity < orderItem.Quantity)
        {
            isStockAvailable = false;
            outOfStockItems.Add(orderItem);
        }
    }

    // If stock is available, verify the order and update the status
    if (isStockAvailable)
    {
        order.OrderStatus = "Verified";  // Mark the order as verified

        // Update stock (deduct the ordered quantity)
        foreach (var orderItem in order.OrderItems)
        {
            var drug = orderItem.Drug;
            drug.Quantity -= orderItem.Quantity;
        }

        await context.SaveChangesAsync();  // Save the updates to the database

        // Send email to the user notifying them about the order verification
        var emailBody = $"<p>Your order with Order ID {order.OrderId} has been successfully verified and is now in processing.</p>";
        var userEmail = order.User.EmailId;

        // Check if user email is null
        if (string.IsNullOrEmpty(userEmail))
        {
            throw new InvalidOperationException("User email is missing.");
        }

        var mailRequest = new MailRequestDTO
        {
            ToEmail = userEmail,
            Subject = "Order Verified",
            Body = emailBody
        };
         mailService.SendEmail(mailRequest);

        return "Order has been verified successfully.";
    }
    else
    {
        // If stock is not available, mark the order as cancelled and notify the user
        order.OrderStatus = "Cancelled";  // Mark the order as cancelled

        await context.SaveChangesAsync();  // Save the updates to the database

        // Send email to the user notifying them about the cancellation
        var emailBody = $"<p>Unfortunately, your order with Order ID {order.OrderId} has been cancelled due to insufficient stock of some items. We apologize for the inconvenience.</p>";
        var userEmail = order.User.EmailId;

        if (string.IsNullOrEmpty(userEmail))
        {
            throw new InvalidOperationException("User email is missing.");
        }

        var mailRequest = new MailRequestDTO
        {
            ToEmail = userEmail,
            Subject = "Order Cancelled",
            Body = emailBody
        };
         mailService.SendEmail(mailRequest);

        return "Order has been cancelled due to insufficient stock.";
    }
}


}
