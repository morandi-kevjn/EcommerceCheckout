using System.Text.Json;
using EcommerceCheckout.Web.Models.ViewModels;
using EcommerceCheckout.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceCheckout.Web.Controllers;

public class CheckoutController : Controller
{
    private readonly ICartServices _cartServices;
    private readonly ICartCookiesAccessor _cartCookiesAccessor;

    public CheckoutController(ICartServices cartServices, ICartCookiesAccessor cartCookiesAccessor)
    {
        _cartServices = cartServices;
        _cartCookiesAccessor = cartCookiesAccessor;
    }

    [HttpGet("/Checkout")]
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
}