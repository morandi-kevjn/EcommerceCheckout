using EcommerceCheckout.Web.Data;
using EcommerceCheckout.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceCheckout.Web.Controllers;

public class PaymentController : Controller
{
    private readonly IOrderService _orderService;
    private readonly IPaymentServiceFactory _paymentServiceFactory;
    private readonly ICartCookiesAccessor  _cartCookieAccessor;

    public PaymentController(IOrderService orderService, IPaymentServiceFactory paymentServiceFactory, ICartCookiesAccessor cartCookieAccessor)
    {
        _orderService = orderService;
        _paymentServiceFactory = paymentServiceFactory;
        _cartCookieAccessor = cartCookieAccessor;
    }

    private async Task<IActionResult> ConfirmAndRedirectAsync(string orderNumber, string providerReferenceId)
    {
        var order = await _orderService.GetByOrderNumberAsync(orderNumber);
        if (order is null)
            return NotFound();

        var providerType = order.PaymentProvider;
        var confirmed = await _paymentServiceFactory.Resolve(providerType).ConfirmPaymentAsync(order, providerReferenceId);

        if (confirmed)
        {
            await _orderService.MarkAsPaidAsync(order);
            _cartCookieAccessor.ClearToken(Response);
            return RedirectToAction("Success", "Checkout", new { orderNumber = order.OrderNumber });
        }

        return Content("Il pagamento non é stato confermato.");
    }

    [HttpGet("/payment/stripe/return")]
    public Task<IActionResult> StripeReturn(string orderNumber, string session_id)
        => ConfirmAndRedirectAsync(orderNumber, session_id);

    [HttpGet("/payment/paypal/return")]
    public Task<IActionResult> PayPalReturn(string orderNumber, string token)
        => ConfirmAndRedirectAsync(orderNumber, token);
}