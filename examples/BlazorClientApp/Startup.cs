using Blazor.Realm;
using Blazor.Realm.Async.Extensions;
using Blazor.Realm.Extensions;
using Blazor.Realm.ReduxDevTools.Extensions;
using BlazorClientApp.Redux;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BlazorClientApp
{
    public class Startup
    {

        private IServiceCollection _services;
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddSingleton<IJSRuntime>();
            services.AddRealmReduxDevToolServices();
            services.AddRealmStore<AppState>(new AppState(), Reducers.RootReducer);

            _services = services;
        }

        public void Configure(IComponentsApplicationBuilder app, IStoreBuilder<AppState> RealmStoreBuilder)
        {
            RealmStoreBuilder.UseRealmAsync<AppState>();

            RealmStoreBuilder.UseRealmReduxDevTools<AppState>(new System.Type[]
            {
                typeof(Redux.Actions.Counter.Dispose)
            });

            //app.Map("/services", builder => builder.Run(async context =>
            //{
            //var sb = new StringBuilder();
            //sb.Append("<h1>All Services</h1>");
            //sb.Append("<table><thead>");
            //sb.Append("<tr><th>Type</th><th>Lifetime</th><th>Instance</th></tr>");
            //sb.Append("</thead><tbody>");
            Console.WriteLine("Test");
            foreach (var svc in _services)
            {
                Console.WriteLine($"{svc.ServiceType.FullName} = {svc.ImplementationType?.FullName}");
            }
                //sb.Append("</tbody></table>");
                //await context.Response.WriteAsync(sb.ToString());
            //}));

            app.AddComponent<App>("app");
        }
    }
}
