using Microsoft.AspNetCore.Mvc;
using Models;

namespace Sección_12___Dependency_Injection.ViewComponents
{
    public class CityWeatherViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(CityWeather city)
        {
            string backgroundClass;

            if (city.TemperatureFahrenheit < 44)
            {
                backgroundClass = "blue-back";
            }
            else if (city.TemperatureFahrenheit <= 74)
            {
                backgroundClass = "green-back";
            }
            else
            {
                backgroundClass = "orange-back";
            }

            ViewBag.BackgroundClass = backgroundClass;

            return View(city);
        }
    }
}
