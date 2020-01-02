using Blazor.Realm;
using Blazor.Realm.Async;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

namespace BlazorServerApp.Redux.Actions
{
    namespace Counter
    {
        public class IncrementByValue : IRealmAction
        {
            public int Value { get; set; }
            public IncrementByValue(int value)
            {
                Value = value;
            }
        }

        public class DecrementByValue : IRealmAction
        {
            public int Value { get; set; }
            public DecrementByValue(int value)
            {
                Value = value;
            }
        }

        public class Reset : IRealmAction { }

        public class Dispose : Reset { }
    }

    namespace WeatherForecasts
    {
        public class Clear : IRealmAction { }

        public class Set : IRealmAction
        {
            public IEnumerable<WeatherForecast> Value { get; set; }
            public Set(IEnumerable<WeatherForecast> forecasts)
            {
                Value = forecasts;
            }
        }

        public class FetchAsync : IAsyncRealmAction
        {
            public Store<AppState> Store { get; set; }
            public HttpClient Client { get; set; }

            public FetchAsync(Store<AppState> store, HttpClient httpClient)
            {
                Store = store ?? throw new ArgumentNullException(nameof(store));
                Client = httpClient ?? throw new ArgumentNullException(nameof(store));
            }

            public async Task Invoke()
            {
                Store.Dispatch(new IsLoading.Set(true));
                Store.Dispatch(new WeatherForecasts.Clear());

                await Task.Delay(1000);
                WeatherForecast[] forecasts = JsonSerializer.Deserialize<WeatherForecast[]>(await (await Client.GetAsync("sample-data/weather.json")).Content.ReadAsStringAsync());

                Store.Dispatch(new WeatherForecasts.Set(forecasts));
                Store.Dispatch(new IsLoading.Set(false));
            }
        }
    }

    namespace IsLoading
    {
        public class Set : IRealmAction
        {
            public bool Value { get; set; }

            public Set(bool value)
            {
                Value = value;
            }
        }
    }
}
