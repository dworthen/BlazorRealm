using Blazor.Realm;
using Blazor.Realm.Async;
using Microsoft.AspNetCore.Blazor;
using Blazor.Realm.ReduxDevTools;
using Microsoft.AspNetCore.Blazor.Browser.Rendering;
using Microsoft.AspNetCore.Blazor.Browser.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorStandAlone
{
    public class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new BrowserServiceProvider(services =>
            {
                // Add any custom services here
                services.AddRealmStore<AppState>(new AppState(), Reducers.RootReducer);
            });

            IStoreBuilder<AppState> RealmStoreBuilder = serviceProvider.GetService<IStoreBuilder<AppState>>();

            //RealmStoreBuilder.UseMiddleware<AppState, HandleAsyncActions<AppState>>();
            RealmStoreBuilder.UseRealmAsync<AppState>();

            RealmStoreBuilder.UseRealmReduxDevTools<AppState>(new System.Type[] 
            {
                typeof(Actions.Counter.Dispose)
            });

            new BrowserRenderer(serviceProvider).AddComponent<App>("app");
        }
    }
}
