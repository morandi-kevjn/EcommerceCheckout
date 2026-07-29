using EcommerceCheckout.Web.Models.Entities;

namespace EcommerceCheckout.Web.Services.Interfaces;

public interface IPaymentServiceFactory
{
    IPaymentService Resolve(PaymentProviderType provider);
}