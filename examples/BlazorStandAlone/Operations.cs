using Blazor.Realm;
using Blazor.Realm.Async;
using Microsoft.AspNetCore.Blazor;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorStandAlone.Operations
{

    namespace Counter
    {
        public class IncrementByValue : IAsyncRealmAction
        {
            public Store<AppState> Store { get; set; }
            public int IncrementAmount { get; set; }
            public IncrementByValue(Store<AppState> store, int incrementAmount = 1)
            {
                Store = store ?? throw new ArgumentNullException(nameof(store));
                IncrementAmount = incrementAmount;
            }
            public async Task Invoke()
            {
                Store.Dispatch(new Actions.Loading.Start());
                await Task.Delay(3000);
                Store.Dispatch(new Actions.Counter.IncrementByValue(IncrementAmount));
                Store.Dispatch(new Actions.Loading.Complete());
            }
        }

    }

    namespace WeatherForecasts
    {
        public class Fetch : IAsyncRealmAction
        {
            public Store<AppState> Store { get; set; }
            public HttpClient Client { get; set; }

            public Fetch(Store<AppState> store, HttpClient httpClient)
            {
                Store = store ?? throw new ArgumentNullException(nameof(store));
                Client = httpClient ?? throw new ArgumentNullException(nameof(store));
            }

            public async Task Invoke()
            {
                Store.Dispatch(new Actions.Loading.Start());
                Store.Dispatch(new Actions.WeatherForecasts.Clear());

                await Task.Delay(3000);
                WeatherForecast[] forecasts = await Client.GetJsonAsync<WeatherForecast[]>("/sample-data/weather.json");

                Store.Dispatch(new Actions.WeatherForecasts.Set(forecasts));
                Store.Dispatch(new Actions.Loading.Complete());
            }
        }
    }


}
