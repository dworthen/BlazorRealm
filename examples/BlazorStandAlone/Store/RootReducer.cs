using Blazor.Realm;
using BlazorStandAlone.Models;
using System;

namespace BlazorStandAlone.Store
{
    public static class RootReducer
    {
        public static AppState Reduce(AppState appState, IAction action)
        {
            if(appState == null)
            {
                throw new ArgumentNullException(nameof(appState));
            }

            return new AppState
            {
                Loading = ReduceLoading(appState.Loading, action),
                Count = Counter.Reducer.Reduce(appState.Count, action),
                WeatherForecasts = WeatherForecasts.Reducer.Reduce(appState.WeatherForecasts, action)
            };
        }

        private static bool ReduceLoading(bool loading, IAction action)
        {
            switch(action)
            {
                case StartLoading _:
                    return true;
                case EndLoading _:
                    return false;
                default:
                    return loading;
            }
        }
    }
}
