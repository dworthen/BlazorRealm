using BlazorRealm;
using BlazorStandAlone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Count = Counter.Reducer.Reduce(appState.Count, action),
                WeatherForecasts = new List<WeatherForecast>()
            };
        }
    }
}
