using EcommerceCheckout.Web.Data;
using EcommerceCheckout.Web.Models.Entities;
using EcommerceCheckout.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EcommerceCheckout.Web.Services.Implementations;

public class CartService : ICartServices
{
    private readonly ApplicationDbContext _db;

    public CartService(ApplicationDbContext db)
    {
        _db = db;
    }
    
    public async Task<Cart> GetOrCreateCartAsync(Guid cartToken)
    {
        var cart = await _db.Carts
            .Include(c => c.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.CartToken == cartToken);

        if (cart is not null)
            return cart;
        
        cart = new Cart { CartToken = cartToken };
        _db.Carts.Add(cart);
        await _db.SaveChangesAsync();
        
        return cart;
    }

    public async Task AddItemAsync(Guid cartToken, int productId, int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be a positive integer.", nameof(quantity));

        var cart = await GetOrCreateCartAsync(cartToken);
        
        var product = await _db.Products.FindAsync(productId)
            ?? throw new InvalidOperationException($"Product with id {productId} does not exist.");
        
        if (!product.Active)
            throw new InvalidOperationException($"Product with id {productId} is not active.");
        
        var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        var requestedQty = (existingItem?.Quantity ?? 0) + quantity;
        
        if (requestedQty > product.StockQty)
            throw new InvalidOperationException($"Quantity {requestedQty} is larger than {product.StockQty}");

        if (existingItem is null)
        {
            cart.Items.Add(new CartItem
            {
                CartId = cart.Id,
                ProductId = productId,
                Quantity = quantity,
                UnitPrice = product.Price,
            });
        }
        else
        {
            existingItem.Quantity =  requestedQty;
        }
        
        await _db.SaveChangesAsync();
    }
}