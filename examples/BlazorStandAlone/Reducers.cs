using Blazor.Realm;
using System;
using System.Collections.Generic;

namespace BlazorStandAlone
{
    public static class Reducers
    {
        public static AppState RootReducer(AppState appState, IAction action)
        {
            if(appState == null)
            {
                throw new ArgumentNullException(nameof(appState));
            }

            return new AppState
            {
                Loading = ReduceLoading(appState.Loading, action),
                Count = CountReducer(appState.Count, action),
                WeatherForecasts = WeatherForecastsReducer(appState.WeatherForecasts, action)
            };
        }

        private static bool ReduceLoading(bool loading, IAction action)
        {
            switch(action)
            {
                case Actions.Loading.Start _:
                    return true;
                case Actions.Loading.Complete _:
                    return false;
                default:
                    return loading;
            }
        }

        public static int CountReducer(int count, IAction action)
        {
            switch (action)
            {
                case Actions.Counter.IncrementByOne _:
                    return count + 1;
                case Actions.Counter.IncrementByValue a:
                    return count + a.Value;
                case Actions.Counter.DecrementByOne _:
                    return count - 1;
                case Actions.Counter.DecrementByValue a:
                    return count - a.Value;
                case Actions.Counter.Reset _:
                    return 0;
                default:
                    return count;
            }
        }

        public static IEnumerable<WeatherForecast> WeatherForecastsReducer(IEnumerable<WeatherForecast> forecasts, IAction action)
        {
            switch (action)
            {
                case Actions.WeatherForecasts.Clear _:
                    return new WeatherForecast[] { };
                case Actions.WeatherForecasts.Set a:
                    return a.WeatherForecasts;
                default:
                    return forecasts;
            }
        }
    }
}
