using EcommerceCheckout.Web.Models.Entities;
using EcommerceCheckout.Web.Services.Interfaces;
using Stripe;
using Stripe.Checkout;

namespace EcommerceCheckout.Web.Services.Implementations;

public class StripePaymentService : IPaymentService
{
    private readonly string _secretKey;

    public StripePaymentService(IConfiguration configuration)
    {
        _secretKey = configuration["Stripe:SecretKey"]
                     ?? throw new InvalidOperationException("Stripe:SecretKey not configured");
    }
    
    public PaymentProviderType Provider => PaymentProviderType.Stripe;

    public async Task<PaymentInitResult> CreatePaymentAsync(Order order, string successUrl, string cancelUrl)
    {
        StripeConfiguration.ApiKey = _secretKey;

        var options = new SessionCreateOptions
        {
            Mode = "payment",
            PaymentMethodTypes = new List<string> { "card" },
            LineItems = order.Items.Select(item => new SessionLineItemOptions
            {
                Quantity = item.Quantity,
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = order.Currency.ToLowerInvariant(),
                    UnitAmount = (long)Math.Round(item.UnitPrice * 100, MidpointRounding.AwayFromZero),
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = item.ProductName,
                    }
                }
            }).ToList(),
            SuccessUrl = successUrl + "?session_id={CHECKOUT_SESSION_ID}",
            CancelUrl = cancelUrl,
            ClientReferenceId = order.OrderNumber
        };

        var service = new SessionService();
        Session session = await service.CreateAsync(options);
        
        return new PaymentInitResult { RedirectUrl = session.Url, ProviderReferenceId = session.Id };
    }

    public async Task<bool> ConfirmPaymentAsync(Order order, string providerReferenceId)
    {
        StripeConfiguration.ApiKey = _secretKey;
        
        var service = new SessionService();
        Session session = await service.GetAsync(providerReferenceId);
        
        return session.PaymentStatus == "paid";
    }
}