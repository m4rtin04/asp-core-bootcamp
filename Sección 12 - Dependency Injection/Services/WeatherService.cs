using Models;
using ServiceContracts;

namespace Services
{
    public class WeatherService : IWeatherService
    {
        private List<CityWeather> _cities = new List<CityWeather>()
        {
            new () { CityUniqueCode = "LDN", CityName = "London", DateAndTime = DateTime.Parse("2030-01-01 8:00"), TemperatureFahrenheit = 33 },
            new () { CityUniqueCode = "NYC", CityName = "New York", DateAndTime = DateTime.Parse("2030-01-01 3:00"), TemperatureFahrenheit = 60 },
            new () { CityUniqueCode = "PAR", CityName = "Paris", DateAndTime = DateTime.Parse("2030-01-01 9:00"), TemperatureFahrenheit = 82 }
        };

       public List<CityWeather> GetWeatherDetails()
        {
            return _cities;
        }

        public CityWeather? GetWeatherByCityCode(string CityCode)
        {
            return _cities.FirstOrDefault(temp => temp.CityUniqueCode.Equals(CityCode));
        }

    }
}
