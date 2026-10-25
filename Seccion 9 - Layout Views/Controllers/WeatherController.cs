using Microsoft.AspNetCore.Mvc;
using Seccion_9___Layout_Views.Models;

namespace Seccion_9___Layout_Views.Controllers
{
    public class WeatherController : Controller
    {
        private readonly List<CityWeather> _cities = new List<CityWeather>()
            {
                new CityWeather() { CityUniqueCode = "LDN", CityName = "London", DateAndTime = Convert.ToDateTime("2030-01-01 8:00"), TemperatureFahrenheit = 33 },
                new CityWeather() { CityUniqueCode = "NYC", CityName = "New York", DateAndTime = Convert.ToDateTime("2030-01-01 3:00"), TemperatureFahrenheit = 60 },
                new CityWeather() { CityUniqueCode = "PAR", CityName = "Paris", DateAndTime = Convert.ToDateTime("2030-01-01 9:00"), TemperatureFahrenheit = 82 }
            };

        [HttpGet("/")]
        public IActionResult Index()
        {
            return View(_cities);
        }

        [HttpGet("/weather/{cityCode}")]
        public IActionResult Details(string? cityCode)
        {
            if (string.IsNullOrEmpty(cityCode))
            {
                return View("Index", _cities);
            }

            var city = _cities.FirstOrDefault(c => c.CityUniqueCode == cityCode);

            if (city == null)
            {
                return Content("Not Foun");
            }

            return View(city);
        }
    }
}
