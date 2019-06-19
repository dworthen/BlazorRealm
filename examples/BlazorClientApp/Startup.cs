using Blazor.Realm;
using Blazor.Realm.Async.Extensions;
using Blazor.Realm.Extensions;
using Blazor.Realm.ReduxDevTools.Extensions;
using BlazorClientApp.Redux;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace BlazorClientApp
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddSingleton<IJSRuntime>();
            services.AddRealmReduxDevToolServices();
            services.AddRealmStore<AppState>(new AppState(), Reducers.RootReducer);
        }

        public void Configure(IComponentsApplicationBuilder app, IStoreBuilder<AppState> RealmStoreBuilder)
        {
            RealmStoreBuilder.UseRealmAsync<AppState>();

            RealmStoreBuilder.UseRealmReduxDevTools<AppState>(new System.Type[]
            {
                typeof(Redux.Actions.Counter.Dispose)
            });

            app.AddComponent<App>("app");
        }
    }
}
