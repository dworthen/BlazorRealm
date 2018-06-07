using BlazorRealm;
using BlazorStandAlone.Models;
using System;
using System.Collections.Generic;

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
                WeatherForecasts = new List<WeatherForecast>()
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
