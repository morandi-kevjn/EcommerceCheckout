using Microsoft.AspNetCore.Mvc;

namespace EcommerceCheckout.Web.Controllers;

public class StaticPagesController : Controller
{
    [HttpGet("/terms")]
    public IActionResult Terms() => View();
    
    [HttpGet("/privacy")]
    public IActionResult Privacy() => View();
}