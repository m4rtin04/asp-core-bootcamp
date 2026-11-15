using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace StockMarket.UI.Controllers;

public class HomeController : Controller
{
    [Route("/Home/Error")]
    public IActionResult Error()
    {
        var feature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        ViewBag.ErrorMessage = feature?.Error?.Message;
        return View();
    }
}
