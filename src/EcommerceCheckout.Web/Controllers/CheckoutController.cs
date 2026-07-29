using System.Text.Json;
using EcommerceCheckout.Web.Models.ViewModels;
using EcommerceCheckout.Web.Models.Entities;
using EcommerceCheckout.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceCheckout.Web.Controllers;

public class CheckoutController : Controller
{
    private readonly ICartServices _cartServices;
    private readonly ICartCookiesAccessor _cartCookiesAccessor;
    private readonly IOrderService _orderService;
    private readonly IPaymentServiceFactory _paymentServiceFactory;

    public CheckoutController(ICartServices cartServices, ICartCookiesAccessor cartCookiesAccessor, IOrderService orderService, IPaymentServiceFactory paymentServiceFactory)
    {
        _cartServices = cartServices;
        _cartCookiesAccessor = cartCookiesAccessor;
        _orderService = orderService;
        _paymentServiceFactory = paymentServiceFactory;
    }

    [HttpGet("/checkout")]
     public async Task<IActionResult> Index()
     {
         var stored = HttpContext.Session.GetString(UserInfoController.SessionKey);
         if (stored is null)
             return RedirectToAction("Index", "UserInfo");
         
         var userInfo = JsonSerializer.Deserialize<UserInfoInputModel>(stored)!;
         
         var existingToken = _cartCookiesAccessor.ReadToken(Request);
         if (existingToken is null)
             return RedirectToAction("Index", "Cart");
         
         var cartSummary = await _cartServices.GetCartSummaryAsync(existingToken.Value);

         var viewModel = new CheckoutPageViewModel
         {
             UserInfo = userInfo,
             Cart = cartSummary
         };
         
         return View(viewModel);
     }

    [HttpPost("/checkout")]
    public async Task<IActionResult> Index(string paymentType)
    {
        var stored = HttpContext.Session.GetString(UserInfoController.SessionKey);
        if (stored is null)
            return RedirectToAction("Index", "UserInfo");
        
        var userInfo = JsonSerializer.Deserialize<UserInfoInputModel>(stored)!;
        
        var existingToken = _cartCookiesAccessor.ReadToken(Request);
        if (existingToken is null)
            return RedirectToAction("Index", "Cart");
        
        if (string.IsNullOrEmpty(paymentType))
        {
            ModelState.AddModelError("PaymentType", "Seleziona un metodo di pagamento.");
            
            var cartSummary = await _cartServices.GetCartSummaryAsync(existingToken.Value);
            return View(new CheckoutPageViewModel
            {
                UserInfo = userInfo,
                Cart = cartSummary
            });
        }
        
        var order = await _orderService.CreateOrderFromCartAsync(existingToken.Value, userInfo, paymentType);
        var providerType = order.PaymentProvider;
        
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var providerSegment = providerType == PaymentProviderType.Stripe ? "stripe" : "paypal";
        var successUrl = $"{baseUrl}/payment/{providerSegment}/return?orderNumber={order.OrderNumber}";
        var cancelUrl = $"{baseUrl}/checkout/cancelled";

        var initResult = await _paymentServiceFactory.Resolve(providerType).CreatePaymentAsync(order, successUrl, cancelUrl);
        
        HttpContext.Session.Remove(UserInfoController.SessionKey);
        
        return Redirect(initResult.RedirectUrl);
    }

    [HttpGet("/checkout/success")]
    public async Task<IActionResult> Success(string orderNumber)
    {
        var order = await _orderService.GetByOrderNumberAsync(orderNumber);
        if (order is null)
            return RedirectToAction("Index", "Cart");
        
        return View(order);
    }

    [HttpGet("/checkout/cancelled")] 
    public IActionResult Cancelled() => View();
}