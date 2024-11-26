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
            <p>Your order has been placed successfully! Your order is currently awaiting approval.</p>
            <p><strong>Order ID:</strong> {order.OrderId}</p>
            <p><strong>Order Date:</strong> {order.OrderDate:yyyy-MM-dd HH:mm:ss}</p>
            <p><strong>Status:</strong> Awaiting Approval</p>
            
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
            <p>We will notify you once your order is approved and ready for shipping.</p>
            <p>Best regards,<br/>Pharmacy system</p>
        </body>
        </html>";

        return emailBody;
    } 
    

    public async Task<string> PlaceOrder(int userId, PlaceOrderDto placeOrderDto)
    {
        var cart = await context.DrugsCarts
                    .Include(c => c.CartItems)  // Include related CartItems
                    .ThenInclude(ci => ci.Drug) // Include the related Drugs (DrugDetails)
                    .FirstOrDefaultAsync(c => c.UserId == userId);  // Get the user's cart

        if (cart == null || cart.CartItems.Count == 0)
        {
            throw new InvalidOperationException("Your cart is empty.");
        }

        
        var orderItems = new List<OrderItem>();
        decimal totalAmountPaid = 0;

        
        foreach (var cartItem in cart.CartItems)
        {
            var drug = cartItem.Drug;
            if (drug == null)
            {
                throw new KeyNotFoundException($"Drug with ID {cartItem.DrugId} not found.");
            }

            var orderItem = new OrderItem
            {
                DrugId = cartItem.DrugId,
                Quantity = cartItem.Quantity,
                Price = cartItem.Price
            };

            totalAmountPaid += orderItem.Quantity * orderItem.Price;  // Calculate total amount here
            orderItems.Add(orderItem);
        }

        
        var order = new OrderDetails
        {
            UserId = userId,
            OrderDate = DateTime.UtcNow,
            OrderStatus = "Complete",
            ShippingAddress = placeOrderDto.ShippingAddress,
            OrderQuantity = cart.CartItems.Sum(ci => ci.Quantity),
            DeliveryDate = DateTime.UtcNow.AddDays(2)
        };

        
        await context.OrderDetails.AddAsync(order);
        await context.SaveChangesAsync(); 

       
        foreach (var orderItem in orderItems)
        {
            orderItem.OrderId = order.OrderId;  
        }

        
        await context.OrderItems.AddRangeAsync(orderItems);
        await context.SaveChangesAsync();

        
        var validTransactionTypes = new List<string> { "UPI", "CreditCard", "DebitCard" };

        if (!validTransactionTypes.Contains(placeOrderDto.TransactionType, StringComparer.OrdinalIgnoreCase))
        {
            return "No other transaction type available";
        }

        
        var transaction = new TransactionDetails
        {
            OrderId = order.OrderId,
            TransactionType = placeOrderDto.TransactionType,
            TransactionStatus = "Paid",
            AmountPaid = totalAmountPaid,  
            TransactionDate = DateTime.UtcNow
        };

        await context.TransactionDetails.AddAsync(transaction);
        await context.SaveChangesAsync();

       
        context.CartItems.RemoveRange(cart.CartItems);
        await context.SaveChangesAsync();

        context.DrugsCarts.Remove(cart);
        await context.SaveChangesAsync();

        
        var emailBody = GenerateOrderConfirmationEmail(order, orderItems);
        var user = await context.UserDetails.FirstOrDefaultAsync(u => u.UserId == userId);

        if (user == null)
        {
            return "User not found.";
        }

        var userEmail = user.EmailId;
        if (string.IsNullOrEmpty(userEmail))
        {
            throw new InvalidOperationException("User email not found.");
        }

        var mailRequest = new MailRequestDTO
        {
            ToEmail = userEmail,
            Subject = "Order Confirmation",
            Body = emailBody
        };
        mailService.SendEmail(mailRequest);

        // Update drug quantities
        foreach (var orderItem in order.OrderItems)
        {
            var drug = orderItem.Drug;
            drug.Quantity -= orderItem.Quantity;
        }
        await context.SaveChangesAsync();

        return "Order placed successfully";
    }
    public async Task<List<OrderCancelDto>> GetUserOrders(int userId)//user can see their orders 
    {
            var orders = await context.OrderDetails
                .Where(o => o.UserId == userId)  // Filter by UserId
                .Include(o => o.OrderItems)      // Include order items
                .ThenInclude(oi => oi.Drug)     // Include drugs associated with order items
                .Select(o => new OrderCancelDto        // Select relevant order details
                {
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate,
                    OrderStatus = o.OrderStatus,
                    DeliveryDate = o.DeliveryDate,
                    TransactionStatus = context.TransactionDetails
                        .Where(t => t.OrderId == o.OrderId)
                        .Select(t => t.TransactionStatus)
                        .FirstOrDefault()
                })
                .ToListAsync();

            return orders;
        }

    public async Task<List<AllOrderDetails>> GetAllOrdersWithDrugs()
    {
            try
            {
                 var orders = await context.OrderDetails
                            .Include(o => o.OrderItems)  
                            .ThenInclude(oi => oi.Drug)  
                            .Join(context.TransactionDetails, 
                                order => order.OrderId,  
                                transaction => transaction.OrderId,
                                (order, transaction) => new { order, transaction })  
                            .Where(o => o.order.OrderStatus != "Verified")  
                            .ToListAsync();


                var orderDtos = orders.Select(o => new AllOrderDetails
                {
                    OrderId = o.order.OrderId,
                    UserId = o.order.UserId,
                    OrderDate = o.order.OrderDate,
                    OrderStatus = o.order.OrderStatus,
                    OrderQuantity = o.order.OrderQuantity,
                    TransactionStatus = o.transaction.TransactionStatus,  
                    OrderItems = o.order.OrderItems.Select(oi => new OrderItemDto
                    {
                        OrderItemId = oi.OrderItemId,
                        DrugId = oi.DrugId,
                        DrugName = oi.Drug.DrugName,  
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
    public async Task<string> ApproveOrder(int orderId)
    {
        try
        {
            var order = await context.OrderDetails
                .Include(o => o.OrderItems)  
                .ThenInclude(oi => oi.Drug)  
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return "Order not found!";
            }

            bool isOrderApproved = true;  
           

            foreach (var orderItem in order.OrderItems)
            {
                
                var drug = await context.DrugDetails.FirstOrDefaultAsync(d => d.DrugId == orderItem.DrugId);

                if (drug == null)
                {
                    isOrderApproved = false;
                    continue; 
                }

                if (orderItem.Quantity > drug.Quantity)
                {
                   
                    isOrderApproved = false;
                }
            }
            if (isOrderApproved)
            {
                order.OrderStatus = "Approved";  
            }
            else
            {
                order.OrderStatus = "Rejected";  
            }

            await context.SaveChangesAsync();

            if (isOrderApproved)
            {
               var user = await context.UserDetails.FirstOrDefaultAsync(u => u.UserId == order.UserId);

                if (user == null)
                {
                    return "User not found.";
                }

                var emailBody = $@"
                    <p>Dear {user.UserName},</p>
                    <p>Your order with Order ID {order.OrderId} has been approved successfully.</p>
                    <p><strong>Shipping Address:</strong> {order.ShippingAddress}</p>
                    <p><strong>Delivery Date:</strong> {order.DeliveryDate:yyyy-MM-dd}</p>
                    <p>Thank you for shopping with us!</p>
                    <p>Best regards,<br/>Pharmacy System</p>";

                var mailRequest = new MailRequestDTO
                {
                    ToEmail = user.EmailId,
                    Subject = "Order Approved - Shipping Details",
                    Body = emailBody
                };

                mailService.SendEmail(mailRequest);

                return "Order has been approved successfully and email sent to the user.";
            }
            else
            {
                return $"Order has been rejected";
            }
        }
        catch (Exception ex)
        {
            
            return $"An error occurred while processing the order: {ex.Message}";
        }
    }

    public async Task<string> CancelCompletedOrder(int orderId, int userid)
    {
            //var userId =int.Parse(user.FindFirst("UserId")?.Value);

            var userDetails=await context.UserDetails.Where(u=>u.UserId==userid).FirstOrDefaultAsync();
            var username=userDetails.UserName;
            
            if(userid==null)
            {
                throw new UnauthorizedAccessException("Invalid User ID.");
            }

        
            var order = await context.OrderDetails
                .Where(o => o.OrderId == orderId) 
                .FirstOrDefaultAsync();
            if (order == null)
            {
                return "Order not found";
            }

            var orderid=order.OrderId;
            
            var orderItems = await context.OrderItems
                .Where(oi => oi.OrderId == order.OrderId)
                .ToListAsync();

            
            foreach (var orderItem in orderItems)
            {
                var drug = await context.DrugDetails.FindAsync(orderItem.DrugId);
                if (drug != null)
                {
                    drug.Quantity += orderItem.Quantity;
                }
            }

            
            order.OrderStatus = "Cancelled";  

            var transaction = await context.TransactionDetails
                .Where(t => t.OrderId == order.OrderId)
                .FirstOrDefaultAsync();

            if (transaction == null)
            {
                return "Transaction details not found.";
            }

        
            transaction.TransactionStatus = "Refund in process";
            
            await context.SaveChangesAsync();

            var emailBody = $@"
                <p>Dear {username},</p>
                <p>Your order with Order ID {orderid} has been successfully cancelled as per your request.</p>
                <p>Your payment for this order will be refunded to your original payment method.</p>
                <p>We apologize for the inconvenience, and we hope to serve you better in the future.</p>
                <p>If you have any questions or concerns, feel free to contact our support team.</p>
                <p>Thank you for shopping with us!</p>";


            var mailRequest = new MailRequestDTO
            {
                ToEmail = order.User.EmailId, 
                Subject = "Your Order Has Been Cancelled",
                Body = emailBody
            };

            
            mailService.SendEmail(mailRequest);

            return $"Order {order.OrderId} has been successfully cancelled, stock updated, and email sent to the user.";
    }
}
