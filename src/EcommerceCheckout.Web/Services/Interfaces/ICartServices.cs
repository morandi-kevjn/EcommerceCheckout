using EcommerceCheckout.Web.Models.Entities;

namespace EcommerceCheckout.Web.Services.Interfaces;

public interface ICartServices
{
    Task<Cart> GetOrCreateCartAsync(Guid cartToken);
    Task AddItemAsync(Guid cartToken, int productId, int quantity);
}