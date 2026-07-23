using EcommerceCheckout.Web.Models.Entities;
using EcommerceCheckout.Web.Models.ViewModels;

namespace EcommerceCheckout.Web.Services.Interfaces;

public interface IOrderService
{
    Task<Order> CreateOrderFromCartAsync(Guid cartToken, UserInfoInputModel userInfoInputModel, string paymentType);
}