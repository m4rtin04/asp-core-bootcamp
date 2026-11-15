using Microsoft.AspNetCore.Mvc;

namespace StockMarket.UI.Controllers;

public class HomeController : Controller
{
    [Route("/")]
    public IActionResult Index() => RedirectToAction("Index", "Trade", new { stockSymbol = "MSFT" });

    [Route("Home/Error")]
    public IActionResult Error() => View();
}
