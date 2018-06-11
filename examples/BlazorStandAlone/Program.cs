using Blazor.Realm;
using Blazor.Realm.Async;
using BlazorStandAlone.Models;
using Microsoft.AspNetCore.Blazor.Browser.Rendering;
using Microsoft.AspNetCore.Blazor.Browser.Services;

namespace BlazorStandAlone
{
    public class Program
    {
        static void Main(string[] args)
        {
            Store<AppState> store = null;
            var serviceProvider = new BrowserServiceProvider(services =>
            {
                // Add any custom services here
                store = services.AddRealmStore<AppState>(new AppState(), Store.RootReducer.Reduce);
            });

            store.ApplyMiddleWare(builder =>
            {
                builder.UseUriLogger<AppState>(serviceProvider);

                builder.UseRealmAsync<AppState>();
            });

            new BrowserRenderer(serviceProvider).AddComponent<App>("app");
        }
    }
}
