using Microsoft.AspNetCore.Mvc;

namespace EcommerceCheckout.Web.Controllers;

public class SaticPagesController : Controller
{
    [HttpGet("/terms")]
    public IActionResult Terms() => View();
    
    [HttpGet("/privacy")]
    public IActionResult Privacy() => View();
}