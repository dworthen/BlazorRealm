using Blazor.Realm;
using Blazor.Realm.Async.Extensions;
using Blazor.Realm.Extensions;
using Blazor.Realm.ReduxDevTools.Extensions;
using Microsoft.AspNetCore.Blazor.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorStandAlone
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRealmStore<AppState>(new AppState(), Reducers.RootReducer);
        }

        public void Configure(IBlazorApplicationBuilder app, IStoreBuilder<AppState> RealmStoreBuilder)
        {
            //app.Services.GetService<>

            RealmStoreBuilder.UseRealmAsync<AppState>();

            RealmStoreBuilder.UseRealmReduxDevTools<AppState>(new System.Type[]
            {
                typeof(Actions.Counter.Dispose)
            });

            app.AddComponent<App>("app");
        }
    }
}
