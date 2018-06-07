using System.Collections.Generic;

namespace BlazorStandAlone.Models
{
    public class AppState
    {
        public int Count { get; set; }
        public List<WeatherForecast> WeatherForecasts { get; set; }
    }
}
