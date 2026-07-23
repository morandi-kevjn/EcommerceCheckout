using EcommerceCheckout.Web.Data;
using EcommerceCheckout.Web.Models.Entities;
using EcommerceCheckout.Web.Models.ViewModels;
using EcommerceCheckout.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EcommerceCheckout.Web.Services.Implementations;

public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _db;
    private readonly ICouponService _couponService;
    
    public OrderService(ApplicationDbContext db, ICouponService couponService)
    {
        _db = db;
        _couponService = couponService;
    }

    public async Task<Order> CreateOrderFromCartAsync(Guid cartToken, UserInfoInputModel userInfo, string paymentType)
    {
        var cart = await _db.Carts
            .Include(c => c.Items).ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.CartToken == cartToken)
            ?? throw new InvalidOperationException("Cart not found");

        if (!cart.Items.Any())
            throw new InvalidOperationException("Cart is empty");
        
        var customer = new Customer
        {
            FirstName = userInfo.FirstName,
            LastName = userInfo.LastName,
            Email = userInfo.Email,
            Nation = userInfo.Nation,
            NewsletterOptIn = userInfo.Newsletter,
            RequiresInvoice = userInfo.Invoice,
            FiscalTaxNumber = userInfo.FiscalTaxNumber,
            FiscalCodeNumber = userInfo.FiscalCodeNumber,
            PrivacyAcceptedAt = DateTime.UtcNow
        };
        _db.Customers.Add(customer);
        
        var subtotal = cart.Items.Sum(i => i.UnitPrice * i.Quantity);
        decimal discount = 0;

        if (cart.CouponId is not null)
        {
            var coupon = await _db.Coupons.FindAsync(cart.CouponId);
            if (coupon is not null)
            {
                var (isValid, discountAmount, _) = await _couponService.ValidateAndComputeDiscountAsync(coupon.Code, subtotal);
                if (isValid)
                    discount = discountAmount;
            }
        }

        PaymentProviderType paymentProvider = paymentType.Equals("stripe", StringComparison.OrdinalIgnoreCase)
            ? PaymentProviderType.Stripe
            : PaymentProviderType.PayPal;

        var order = new Order
        {
            OrderNumber = $"ORD-{DateTime.UtcNow:yyyyMMddHHmmss}",
            Customer = customer,
            CartId = cart.Id,
            CouponId = cart.CouponId,
            SubtotalAmount = subtotal,
            DiscountAmount = discount,
            TotalAmount = subtotal - discount,
            Currency = "EUR",
            Status = OrderStatus.PendingPayment,
            PaymentProvider = paymentProvider,
            OrderItems = cart.Items.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                ProductName = i.Product.Name,
                UnitPrice = i.UnitPrice,
                Quantity = i.Quantity,
                LineTotal = i.UnitPrice * i.Quantity
            }).ToList()
        };
        
        _db.Orders.Add(order);
        cart.Status = CartStatus.Converted;
        
        await _db.SaveChangesAsync();
        return order;
    }
}