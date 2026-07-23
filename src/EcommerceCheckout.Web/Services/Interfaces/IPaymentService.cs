using EcommerceCheckout.Web.Models.Entities;

namespace EcommerceCheckout.Web.Services.Interfaces;

public class PaymentInitResult
{
    public string RedirectUrl { get; set; } = string.Empty;
    public string ProviderReferenceId { get; set; } = string.Empty;
}

public interface IPaymentService
{
    PaymentProviderType Provider { get; }
    Task<PaymentInitResult> CreatePaymentAsync(Order order, string successUrl, string cancelUrl);
    Task<bool> ConfirmPaymentAsync(Order order, string providerReferenceId);
}