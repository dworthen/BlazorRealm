using BlazorRealm;
using BlazorStandAlone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
