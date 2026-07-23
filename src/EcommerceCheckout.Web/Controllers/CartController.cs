using EcommerceCheckout.Web.Services.Interfaces;
using EcommerceCheckout.Web.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceCheckout.Web.Controllers;

public record CartAjaxResponse(bool Success, string? Message, object? Cart = null);
public record AddItemRequest(int ProductId, int Quantity);
public record UpdateQuantityRequest(int ProductId, int Quantity);
public record RemoveItemRequest(int ProductId);
public record ApplycouponRequest(string CouponCode);

public class CartController : Controller
{
    private readonly ICartServices _cartServices;
    private readonly ICartCookiesAccessor _cartCookiesAccessor;

    public CartController(ICartServices cartServices, ICartCookiesAccessor cartCookiesAccessor)
    {
        _cartServices = cartServices;
        _cartCookiesAccessor = cartCookiesAccessor;
    }

    private async Task<Cart> GetOrCreateCartWithCookieAsync()
    {
        // 1. Leggo il cookie, se esiste
        Guid? existingToken = _cartCookiesAccessor.ReadToken(Request);
        
        // 2. Ottengo (o creo) il carrello
        var cart = await _cartServices.GetOrCreateCartAsync(existingToken);
        
        // 3. Se il token era nuovo (o diverso da quello letto), scrivo il cookie
        if (existingToken != cart.CartToken)
        {
            _cartCookiesAccessor.WriteToken(Response, cart.CartToken, Request.IsHttps);
        }

        return cart;
    }

    [HttpGet("/cart")]
    public async Task<IActionResult> Index()
    {
        var cart = await GetOrCreateCartWithCookieAsync();
        var summary = await _cartServices.GetCartSummaryAsync(cart.CartToken);
        
        return View(summary);
    }

    [HttpPost("/cart/add")]
    public async Task<IActionResult> AddItem([FromBody] AddItemRequest request)
    {
        var cart = await GetOrCreateCartWithCookieAsync();

        try
        {
            await _cartServices.AddItemAsync(cart.CartToken, request.ProductId, request.Quantity);
            return Json(new CartAjaxResponse(true, null));
        }
        catch (InvalidOperationException ex)
        {
            return Json(new CartAjaxResponse(false, ex.Message));
        }
    }

    [HttpPost("/cart/update")]
    public async Task<IActionResult> UpdateQuantity([FromBody] UpdateQuantityRequest request)
    {
        var cart = await GetOrCreateCartWithCookieAsync();

        try
        {
            await _cartServices.UpdateQuantityAsync(cart.CartToken, request.ProductId, request.Quantity);
            return Json(new CartAjaxResponse(true, null));
        }
        catch (InvalidOperationException ex)
        {
            return Json(new CartAjaxResponse(false, ex.Message));
        }
    }

    [HttpPost("/cart/remove")]
    public async Task<IActionResult> RemoveItem([FromBody] RemoveItemRequest request)
    {
        var cart = await GetOrCreateCartWithCookieAsync();
        await _cartServices.RemoveItemAsync(cart.CartToken, request.ProductId);
        
        return Json(new CartAjaxResponse(true, null));
    }

    [HttpPost("/cart/coupon/apply")]
    public async Task<IActionResult> ApplyCoupon([FromBody] ApplycouponRequest request)
    {
        var cart = await GetOrCreateCartWithCookieAsync();
        var (success, errorMessage) = await _cartServices.ApplyCouponAsync(cart.CartToken, request.CouponCode);
        
        return Json(new CartAjaxResponse(success, errorMessage));
    }
}