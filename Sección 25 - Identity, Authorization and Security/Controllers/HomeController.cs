using Microsoft.AspNetCore.Mvc;

namespace Sección_25___Identity__Authorization_and_Security.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
