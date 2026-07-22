using EcommerceCheckout.Web.Data;
using EcommerceCheckout.Web.Models.Entities;
using EcommerceCheckout.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EcommerceCheckout.Web.Services.Implementations;

public class CartService : ICartServices
{
    private readonly ApplicationDbContext _db;
    private readonly ICouponService _couponService;

    public CartService(ApplicationDbContext db, ICouponService couponService)
    {
        _db = db;
        _couponService = couponService;
    }
    
    public async Task<Cart> GetOrCreateCartAsync(Guid? cartToken)
    {
        if (cartToken is Guid token)
        {
            var existing = await _db.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.CartToken == token);

            if (existing is not null)
                return existing;
        }

        
        var newCart = new Cart { CartToken = cartToken ?? Guid.NewGuid() };
        _db.Carts.Add(newCart);
        await _db.SaveChangesAsync();
        
        return newCart;
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

    public async Task UpdateQuantityAsync(Guid cartToken, int productId, int newQuantity)
    {
        var cart = await GetOrCreateCartAsync(cartToken);
        
        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId)
            ?? throw new InvalidOperationException($"Product with id {productId} does not exist.");

        if (newQuantity <= 0)
        {
            cart.Items.Remove(item);
            _db.CartItems.Remove(item);
        }
        else
        {
            var product = await _db.Products.FindAsync(productId)
                ?? throw new InvalidOperationException($"Product with id {productId} is not active.");
            
            if (product is not null && newQuantity > product.StockQty)
                throw new InvalidOperationException($"Quantity {newQuantity} is larger than {product.StockQty}");
            
            item.Quantity = newQuantity;
        }
        
        await _db.SaveChangesAsync();
    }

    public async Task RemoveItemAsync(Guid cartToken, int productId)
    {
        var cart = await GetOrCreateCartAsync(cartToken);

        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

        if (item is not null)
        {
            cart.Items.Remove(item);
            _db.CartItems.Remove(item);
            await _db.SaveChangesAsync();
        }
    }
    
    public async Task<(bool Success, string? ErrorMessage)> ApplyCouponAsync(Guid cartToken, string couponCode)
    {
        var cart = await GetOrCreateCartAsync(cartToken);
        var subtotal = cart.Items.Sum(i => i.UnitPrice * i.Quantity);
        
        var (isValid, _, errorMessage) = await _couponService.ValidateAndComputeDiscountAsync(couponCode, subtotal);
        
        if (!isValid)
            return (false, errorMessage);
        
        var coupon = await _db.Coupons.FirstAsync(c => c.Code.ToUpper() == couponCode.ToUpperInvariant());
        cart.CouponId = coupon.Id;
        await _db.SaveChangesAsync();
        
        return (true, null);
    }
}