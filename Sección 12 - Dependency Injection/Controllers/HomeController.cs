using Microsoft.AspNetCore.Mvc;
using Models;
using ServiceContracts;

namespace Sección_12___Dependency_Injection.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWeatherService _weatherService;

        public HomeController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [Route("/")]
        public IActionResult Index()
        {
            List<CityWeather> cities = _weatherService.GetWeatherDetails();
            return View(cities);
        }

        [HttpGet("/weather/{cityCode}")]
        public IActionResult Details(string? cityCode)
        {
            CityWeather? city = _weatherService.GetWeatherByCityCode(cityCode);

            if (city == null)
            {
                return Content("Not Foun");
            }

            return View(city);
        }
    }
}
