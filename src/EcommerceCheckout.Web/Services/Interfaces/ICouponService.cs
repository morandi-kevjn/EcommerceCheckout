namespace EcommerceCheckout.Web.Services.Interfaces;

public interface ICouponService
{
    Task<(bool IsValid, decimal DiscountAmount, string? ErrorMessage)> ValidateAndComputeDiscountAsync(
        string code, decimal cartSubtotal);
}