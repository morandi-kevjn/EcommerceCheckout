using EcommerceCheckout.Web.Models.Entities;

namespace EcommerceCheckout.Web.Services.Interfaces;

public interface ICartServices
{
    Task<Cart> GetOrCreateCartAsync(Guid? cartToken);
    Task AddItemAsync(Guid cartToken, int productId, int quantity);
    Task UpdateQuantityAsync(Guid cartToken, int productId, int newQuantity);
    Task RemoveItemAsync(Guid cartToken, int productId);
    Task<(bool Success, string? ErrorMessage)> ApplyCouponAsync(Guid cartToken, string couponCode);
}