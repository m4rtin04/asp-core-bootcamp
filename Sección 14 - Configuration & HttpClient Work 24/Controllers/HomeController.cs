using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Sección_14___Configuration___HttpClient_Work_24.Options;

namespace Sección_14___Configuration___HttpClient_Work_24.Controllers
{
    public class HomeController : Controller
    {
        private readonly SocialMediaLinksOptions _socialMediaLinksOptions;

        public HomeController(IOptions<SocialMediaLinksOptions> socialMediaLinksOptions)
        {
            _socialMediaLinksOptions = socialMediaLinksOptions.Value;
        }
        [Route("/")]
        public IActionResult Index()
        {
            ViewBag.SocialMediaLinks = _socialMediaLinksOptions;
            return View();
        }
    }
}
