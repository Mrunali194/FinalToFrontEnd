using System.Security.Claims;
using Demo.Dtos;
using Demo.Model;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Repository;

public interface IAddTocart
{

   // Task<string> AddToCartAsync(AddToCartDto addToCartDto, ClaimsPrincipal user);
   // //Task<string> UpdateCartItemQuantityAsync(UpdateCartItemDto updateCartItemDto);
   // Task<string> UpdateCartItemQuantityAsync(UpdateCartItemDto updateCartItemDto);
   
   Task<string> AddToCart(AddToCartDto addToCartDto, ClaimsPrincipal user);
   Task<DrugsCart> GetOrCreateUserCart(int userId);
    Task AddOrUpdateCartItem(DrugsCart cart, AddToCartDto addToCartDto, DrugDetails drug);

    //Task UpdateCartTotals(DrugsCart cart);


}

