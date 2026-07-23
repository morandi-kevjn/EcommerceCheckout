using System.Text.Json;
using EcommerceCheckout.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceCheckout.Web.Controllers;

public class UserInfoController : Controller
{
    public const string SessionKey = "checkout_userinfo";

    [HttpGet("/user-info")]
    public IActionResult Index()
    {
        var model = new UserInfoInputModel();
        var stored = HttpContext.Session.GetString(SessionKey);
        
        if (stored is not null)
            model = JsonSerializer.Deserialize<UserInfoInputModel>(stored);

        return View(model);
    }

    [HttpPost("/user-info")]
    public IActionResult Index(UserInfoInputModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        
        HttpContext.Session.SetString(SessionKey, JsonSerializer.Serialize(model));
        return RedirectToAction("Index", "Checkout");
    }

}