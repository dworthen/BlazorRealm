using Blazor.Realm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor.Browser.Services;
using Microsoft.AspNetCore.Blazor.Services;
using Microsoft.AspNetCore.Blazor;

namespace BlazorStandAlone
{
    public class Test<TState>
    {
        public BrowserServiceProvider ServiceProvider { get; set; }
        public Store<TState> Store { get; set; }
        public Test(BrowserServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            IUriHelper uriHelper = null;
            uriHelper = ServiceProvider.GetService(typeof(IUriHelper)) as IUriHelper;
            Store = ServiceProvider.GetService(typeof(Store<TState>)) as Store<TState>;
            uriHelper.OnLocationChanged += (s, e) =>
            {
                string uri = uriHelper.GetAbsoluteUri();
                string counter = uriHelper.ToAbsoluteUri("/counter").ToString();
                Console.WriteLine("URI Changed {0}", uriHelper.GetAbsoluteUri());
                Console.WriteLine("{0}", counter);
                if(uri != counter)
                {
                    uriHelper.NavigateTo("/counter");
                }
                Console.WriteLine("STore State {0}", JsonUtil.Serialize(Store.State));
            };
        }

        public Dispatcher<TState> UriLogger<TState>(Store<TState> store, Dispatcher<TState> next)
        {
            return (IAction action) =>
            {
                return next(action);
            };
        }
    }

    public static class BuilerExtensions
    {
        public static void UseUriLogger<TState>(this RealmMiddlewareBuilder<TState> builder, BrowserServiceProvider serviceProvider)
        {
            Test<TState> test = new Test<TState>(serviceProvider);
            builder.Use(test.UriLogger);
        } 
    }
}
