using System.Security.Claims;
using Demo.Dtos;
using Demo.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
namespace Demo.Repository;

public interface IOrderRepository
{
     Task<List<AllOrderDetails>> GetAllOrdersWithDrugs();
     
     Task<string> PlaceOrder(int userId,PlaceOrderDto placeOrderDto);
     Task<List<OrderCancelDto>> GetUserOrders(int userId);

     Task<string> CancelCompletedOrder(int orderId, int userid);
     Task<string> ApproveOrder(int orderId);
     
    
}