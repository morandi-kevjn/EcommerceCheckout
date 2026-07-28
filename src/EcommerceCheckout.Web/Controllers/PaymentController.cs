using EcommerceCheckout.Web.Data;
using EcommerceCheckout.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceCheckout.Web.Controllers;

public class PaymentController : Controller
{
    private readonly IOrderService _orderService;
    private readonly IPaymentService _paymentService;
    private readonly ICartCookiesAccessor  _cartCookieAccessor;

    public PaymentController(IOrderService orderService, IPaymentService paymentService, ICartCookiesAccessor cartCookieAccessor)
    {
        _orderService = orderService;
        _paymentService = paymentService;
        _cartCookieAccessor = cartCookieAccessor;
    }

    [HttpGet("/payment/stripe/return")]
    public async Task<IActionResult> StripeReturn(string orderNumber, string session_id)
    {
        var order = await _orderService.GetByOrderNumberAsync(orderNumber);
        if (order is null)
            return NotFound();

        var confirmed = await _paymentService.ConfirmPaymentAsync(order, session_id);

        if (confirmed)
        {
            await _orderService.MarkAsPaidAsync(order);
            _cartCookieAccessor.ClearToken(Response);
            return RedirectToAction("Success", "Checkout", new { orderNumber = order.OrderNumber });
        }

        return Content("Il pagamento non é stato confermato.");
    }
}