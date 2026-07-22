using EcommerceCheckout.Web.Data;
using EcommerceCheckout.Web.Models.Entities;
using EcommerceCheckout.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EcommerceCheckout.Web.Services.Implementations;

public class CouponService : ICouponService
{
    private readonly ApplicationDbContext _db;
    
    public CouponService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<(bool IsValid, decimal DiscountAmount, string? ErrorMessage)> ValidateAndComputeDiscountAsync(
        string code, decimal cartSubtotal)
    {
        var normalized = code.Trim().ToUpperInvariant();
        var coupon = await _db.Coupons.FirstOrDefaultAsync(c => c.Code.ToUpper() == normalized);
        
        if (coupon is null)
            return (false, 0, "Coupon not valid or not active");
        
        var now = DateTime.UtcNow;
        if (coupon.ValidFrom is not null && now < coupon.ValidFrom)
            return (false, 0, $"Coupon {code} not valid");
        
        if (coupon.ValidTo is not null && now > coupon.ValidTo)
            return (false, 0, $"Coupon {code} expired");
        
        if (coupon.UsageLimit is not null && coupon.UsedCount >= coupon.UsageLimit)
            return (false, 0, $"Coupon {code} usage limit reached");
        
        if (cartSubtotal < coupon.MinPrice)
            return (false, 0, $"Coupon {code} requires a minimum cart subtotal of {coupon.MinPrice:C}");
        
        if (cartSubtotal > coupon.MaxPrice)
            return (false, 0, $"Coupon {code} requires a maximum cart subtotal of {coupon.MaxPrice:C}");
        
        var discount = coupon.DiscountType == DiscountType.Percentage
            ? Math.Round(cartSubtotal * (coupon.DiscountValue / 100), 2)
            : Math.Min(coupon.DiscountValue, cartSubtotal);

        return (true, discount, null);
    }
}