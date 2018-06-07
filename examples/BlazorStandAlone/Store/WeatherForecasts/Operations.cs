using BlazorRealm;
using BlazorRealmAsync;
using BlazorStandAlone.Models;
using Microsoft.AspNetCore.Blazor;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using static BlazorStandAlone.Store.WeatherForecasts.Actions;

namespace BlazorStandAlone.Store.WeatherForecasts
{
    public class Operations
    {
        public class FetchWeatherForecasts: IAsyncAction
        {
            public Store<AppState> Store { get; set; }
            public HttpClient Client { get; set; }

            public FetchWeatherForecasts(Store<AppState> store, HttpClient httpClient)
            {
                Store = store ?? throw new ArgumentNullException(nameof(store));
                Client = httpClient ?? throw new ArgumentNullException(nameof(store));
            }

            public async Task Invoke()
            {
                Store.Dispatch(new StartLoading());
                Store.Dispatch(new ClearWeatherForecasts());

                await Task.Delay(3000);
                WeatherForecast[] forecasts = await Client.GetJsonAsync<WeatherForecast[]>("/sample-data/weather.json");

                Store.Dispatch(new SetWeatherForecasts(forecasts));
                Store.Dispatch(new EndLoading());
            }
        }
    }
}
