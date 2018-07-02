using Blazor.Realm;
using Microsoft.AspNetCore.Blazor.Browser.Rendering;
using Microsoft.AspNetCore.Blazor.Browser.Services;
using Microsoft.Extensions.DependencyInjection;
using Blazor.Realm.Extensions;
using Blazor.Realm.Async.Extensions;
using Blazor.Realm.ReduxDevTools.Extensions;

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
