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

            IRealmStoreBuilder<AppState> RealmStoreBuilder = serviceProvider.GetService<IRealmStoreBuilder<AppState>>();

            //RealmStoreBuilder.UseRealmAsync<AppState>();

            //RealmStoreBuilder.Use((Store<AppState> localStore, Dispatcher<AppState> next) => {
            //    return (IAction action) =>
            //    {
            //        System.Console.WriteLine("Testing new mw");
            //        System.Console.WriteLine("State: {0}", JsonUtil.Serialize(localStore.State));
            //        return next(action);
            //    };
            //});

            //RealmStoreBuilder.UseMiddleware<AppState, HandleAsyncActions<AppState>>();
            RealmStoreBuilder.UseRealmAsync<AppState>();

            RealmStoreBuilder.UseRealmReduxDevTools();

            //store.ApplyMiddleWare(builder =>
            //{
            //    builder.UseRealmAsync<AppState>();

            //    builder.UseRealmReduxDevTools<AppState>(serviceProvider, new System.Type[] {
            //        // Ignore Reset actions in Redux DevTools
            //        // browser extension
            //        typeof(Actions.Counter.Dispose)
            //    });
            //});

            new BrowserRenderer(serviceProvider).AddComponent<App>("app");
        }
    }
}
