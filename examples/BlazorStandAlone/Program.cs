using BlazorRealm;
using BlazorStandAlone.Models;
using Microsoft.AspNetCore.Blazor.Browser.Rendering;
using Microsoft.AspNetCore.Blazor.Browser.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BlazorStandAlone
{
    public class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new BrowserServiceProvider(services =>
            {
                // Add any custom services here
                services.AddRealmStore<AppState>(new AppState(), Store.RootReducer.Reduce);
            });

            new BrowserRenderer(serviceProvider).AddComponent<App>("app");
        }
    }
}
