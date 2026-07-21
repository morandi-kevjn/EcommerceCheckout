using EcommerceCheckout.Web.Services.Interfaces;
using EcommerceCheckout.Web.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceCheckout.Web.Controllers;

public class CartController : Controller
{
    private readonly ICartServices _cartServices;

    public CartController(ICartServices cartServices)
    {
        _cartServices = cartServices;
    }

    private Guid? CheckToken(Guid? existingToken)
    {
        if (Request.Cookies.TryGetValue("cartToken", out var raw) && Guid.TryParse(raw, out var parsed))
        {
            existingToken = parsed;
        }
        
        return existingToken;
    }

    private async Task<Cart> GetOrCreateCartWithCookieAsync()
    {
        // 1. Leggo il cookie, se esiste
        Guid? existingToken = null;
        existingToken = CheckToken(existingToken);
        
        // 2. Ottengo (o creo) il carrello
        var cart = await _cartServices.GetOrCreateCartAsync(existingToken);
        
        // 3. Se il token era nuovo (o diverso da quello letto), scrivo il cookie
        if (existingToken != cart.CartToken)
        {
            Response.Cookies.Append("cartToken", cart.CartToken.ToString(), new CookieOptions
            {
                HttpOnly = true,
                Secure = Request.IsHttps, // set to True only if we are in https
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddDays(30)
            });
        }
        
        return cart;
    }

    [HttpGet("/cart")]
    public async Task<IActionResult> Index()
    {
        var cart = await GetOrCreateCartWithCookieAsync();
        
        return View(cart);
    }

    [HttpGet("/cart/add")]
    public async Task<IActionResult> AddItem(int productId, int quantity = 1)
    {
        var cart = await GetOrCreateCartWithCookieAsync();
        await _cartServices.AddItemAsync(cart.CartToken, productId, quantity);
        
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("/cart/update")]
    public async Task<IActionResult> UpdateQuantity(int productId, int quantity)
    {
        var cart = await GetOrCreateCartWithCookieAsync();
        await _cartServices.UpdateQuantityAsync(cart.CartToken, productId, quantity);
        
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("/cart/remove")]
    public async Task<IActionResult> RemoveItem(int productId)
    {
        var cart = await GetOrCreateCartWithCookieAsync();
        await _cartServices.RemoveItemAsync(cart.CartToken, productId);
        
        return RedirectToAction(nameof(Index));
    }
}