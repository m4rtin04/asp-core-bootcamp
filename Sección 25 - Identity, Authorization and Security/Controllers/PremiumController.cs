using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Sección_25___Identity__Authorization_and_Security.Controllers
{
    [Authorize(Policy = "Premium")]
    public class PremiumController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
