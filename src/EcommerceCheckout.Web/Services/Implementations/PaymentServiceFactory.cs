using EcommerceCheckout.Web.Models.Entities;
using EcommerceCheckout.Web.Services.Interfaces;

namespace EcommerceCheckout.Web.Services.Implementations;

public class PaymentServiceFactory : IPaymentServiceFactory
{
    private readonly IEnumerable<IPaymentService> _paymentServices;

    public PaymentServiceFactory(IEnumerable<IPaymentService> paymentServices)
    {
        _paymentServices = paymentServices;
    }

    public IPaymentService Resolve(PaymentProviderType provider)
    {
        return _paymentServices.FirstOrDefault(p => p.Provider == provider)
            ?? throw new NotSupportedException($"Provider '{provider}' is not supported.");
    }
}