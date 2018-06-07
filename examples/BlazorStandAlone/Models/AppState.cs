using System.Collections.Generic;

namespace BlazorStandAlone.Models
{
    public class AppState
    {
        public bool Loading { get; set; } = false;
        public int Count { get; set; }
        public List<WeatherForecast> WeatherForecasts { get; set; } = new List<WeatherForecast>();
    }
}
