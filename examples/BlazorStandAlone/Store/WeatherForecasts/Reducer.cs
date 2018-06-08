using Blazor.Realm;
using BlazorStandAlone.Models;
using System.Collections.Generic;
using static BlazorStandAlone.Store.WeatherForecasts.Actions;

namespace BlazorStandAlone.Store.WeatherForecasts
{
    public static class Reducer
    {
        public static IEnumerable<WeatherForecast> Reduce(IEnumerable<WeatherForecast> forecasts, IAction action)
        {
            switch(action)
            {
                case ClearWeatherForecasts _:
                    return new WeatherForecast[] { };
                case SetWeatherForecasts a:
                    return a.WeatherForecasts;
                default:
                    return forecasts;
            }
        }
    }
}
