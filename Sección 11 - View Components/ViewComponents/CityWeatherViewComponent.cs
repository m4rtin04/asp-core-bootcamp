using Microsoft.AspNetCore.Mvc;
using Sección_11___View_Components.Models;

namespace Sección_11___View_Components.ViewComponents
{
    public class CityWeatherViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(CityWeather city)
        {
            
            return View(city);
        }
    }
}
