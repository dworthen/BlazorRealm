using Blazor.Realm;
using Blazor.Realm.Async;
using Blazor.Realm.ReduxDevTools;
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
                store = services.AddRealmStore<AppState>(new AppState(), Reducers.RootReducer);
            });

            store.ApplyMiddleWare(builder =>
            {
                builder.UseRealmAsync<AppState>();

                builder.UseRealmReduxDevTools<AppState>(serviceProvider, new System.Type[] {
                    // Ignore Reset actions in Redux DevTools
                    // browser extension
                    typeof(Actions.Counter.Reset)
                });
            });

            new BrowserRenderer(serviceProvider).AddComponent<App>("app");
        }
    }
}
