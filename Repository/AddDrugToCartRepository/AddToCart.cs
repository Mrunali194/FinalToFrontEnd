// UserService.cs
using Demo.Dtos;
using Demo.Database;
using Demo.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;


namespace Demo.Repository
{
    public class AddTocart : IAddTocart
    {
        private readonly DatabaseContext context;

        public AddTocart(DatabaseContext _context)
        {
            context = _context;
        }

    public async Task AddOrUpdateCartItem(DrugsCart cart, AddToCartDto addToCartDto, DrugDetails drug)
    {
        // Look for an existing cart item for this drug
        var existingCartItem = await context.CartItems
            .FirstOrDefaultAsync(ci => ci.CartId == cart.CartId && ci.DrugId == addToCartDto.DrugId);

        if (existingCartItem != null)
        {
            // Update the quantity of the existing cart item
            existingCartItem.Quantity += addToCartDto.Quantity;
        }
        else
        {
            // Add a new cart item
            var cartItem = new CartItem
            {
                CartId = cart.CartId,
                DrugId = addToCartDto.DrugId,
                Quantity = addToCartDto.Quantity,
                Price = drug.Price  // Set the current price of the drug
            };

            // Add the new cart item to the database
            await context.CartItems.AddAsync(cartItem);
        }
        }

        public async Task<string> AddToCart(AddToCartDto addToCartDto, ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedAccessException("Invalid User ID.");
            }

            // Find the drug by ID
            var drug = await context.DrugDetails.FindAsync(addToCartDto.DrugId);
            if (drug == null)
            {
                throw new KeyNotFoundException("Drug not found.");
            }

            // Check if the drug's stock is available
            if (drug.Quantity < addToCartDto.Quantity)
            {
                throw new InvalidOperationException("Not enough stock available.");
            }

            // Step 1: Check if the user has an existing cart, if not create one
            var cart = await GetOrCreateUserCart( userId);
            // Step 2: Add or update the CartItem for the drug
            await AddOrUpdateCartItem(cart, addToCartDto, drug);

            cart.Quantity += addToCartDto.Quantity;
            cart.TotalPrice += drug.Price * addToCartDto.Quantity;

            // Save all changes (CartItems, Cart, and Drugs)
            await context.SaveChangesAsync();

            return "Item added to cart successfully!";
        }

    public async Task<DrugsCart> GetOrCreateUserCart(int userId)
    {
            var cart = await context.DrugsCarts.FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                // If no cart exists for the user, create a new cart
                cart = new DrugsCart
                {
                    UserId = userId,
                    Quantity = 0,  // Initial quantity (empty cart)
                    TotalPrice = 0  // Initial price (empty cart)
                };

                // Add the new cart to the context and save
                await context.DrugsCarts.AddAsync(cart);
                await context.SaveChangesAsync();  // Save to generate CartId for the user
            }

            return cart;
        }
    }
}

