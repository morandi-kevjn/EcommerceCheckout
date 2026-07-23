using System.Text.Json;
using EcommerceCheckout.Web.Models.ViewModels;
using EcommerceCheckout.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceCheckout.Web.Controllers;

public class CheckoutController : Controller
{
    private readonly ICartServices _cartServices;
    private readonly ICartCookiesAccessor _cartCookiesAccessor;
    private readonly IOrderService _orderService;

    public CheckoutController(ICartServices cartServices, ICartCookiesAccessor cartCookiesAccessor, IOrderService orderService)
    {
        _cartServices = cartServices;
        _cartCookiesAccessor = cartCookiesAccessor;
        _orderService = orderService;
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
        
        var order = await _orderService.CreateOrderFromCartAsync(existingToken.Value, userInfo, paymentType);
        
        HttpContext.Session.Remove(UserInfoController.SessionKey);
        
        return RedirectToAction("Success", new { orderNumber = order.OrderNumber });
    }

    [HttpGet("/checkout/success")]
    public IActionResult Success(string orderNumber)
    {
        return Content($"Ordine {orderNumber} creato con successo!");
    }
}