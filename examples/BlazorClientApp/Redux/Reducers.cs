using Blazor.Realm;
using System;
using System.Collections.Generic;

namespace BlazorClientApp.Redux
{
    public static class Reducers
    {
        public static AppState RootReducer(AppState appState, IRealmAction action)
        {
            if (appState == null)
            {
                throw new ArgumentNullException(nameof(appState));
            }

            return new AppState
            {
                IsLoading = IsLoadingReducer(appState.IsLoading, action),
                Count = CounterReducer(appState.Count, action),
                WeatherForecasts = WeatherForecastsReducer(appState.WeatherForecasts, action)
            };
        }

        public static int CounterReducer(int count, IRealmAction action)
        {
            switch (action)
            {
                case Actions.Counter.IncrementByValue a:
                    return count + a.Value;
                case Actions.Counter.DecrementByValue a:
                    return count - a.Value;
                case Actions.Counter.Dispose _:
                case Actions.Counter.Reset _:
                    return 0;
                default:
                    return count;            }
        }

        public static IEnumerable<WeatherForecast> WeatherForecastsReducer(IEnumerable<WeatherForecast> weatherForecasts, IRealmAction action)
        {
            switch (action)
            {
                case Actions.WeatherForecasts.Set a:
                    return a.Value;
                case Actions.WeatherForecasts.Clear _:
                    return new List<WeatherForecast>();
                default:
                    return weatherForecasts;
            }
        }

        public static bool IsLoadingReducer(bool isLoading, IRealmAction action)
        {
            switch(action)
            {
                case Actions.IsLoading.Set a:
                    return a.Value;
                default:
                    return isLoading;
            }
        }
    }
}
