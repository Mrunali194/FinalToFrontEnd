using System.Security.Claims;
using Demo.Dtos;
using Demo.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
namespace Demo.Repository;

public interface IOrderRepository
{
     Task<List<OrderWithDrugsDto>> GetAllOrdersWithDrugs();
     Task<string> PlaceOrder(ClaimsIdentity user);
     Task<string> VerifyOrder(int orderId);
    
}