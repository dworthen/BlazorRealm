using BlazorRealm;
using BlazorStandAlone.Models;
using System.Collections.Generic;

namespace BlazorStandAlone.Store.WeatherForecasts
{
    public class Actions
    {
        public class ClearWeatherForecasts : IAction { }

        public class SetWeatherForecasts : IAction
        {
            public IEnumerable<WeatherForecast> WeatherForecasts { get; set; }
            public SetWeatherForecasts(IEnumerable<WeatherForecast> forecasts)
            {
                WeatherForecasts = forecasts;
            }
        }
    }
}
