using Blazor.Realm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor.Browser.Services;
using Microsoft.AspNetCore.Blazor.Services;

namespace BlazorStandAlone
{
    public class Test
    {
        public BrowserServiceProvider ServiceProvider { get; set; }
        public Test(BrowserServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            IUriHelper uriHelper = null;
            uriHelper = ServiceProvider.GetService(typeof(IUriHelper)) as IUriHelper;
            uriHelper.OnLocationChanged += (s, e) =>
            {
                Console.WriteLine("URI Changed");
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
            Test test = new Test(serviceProvider);
            builder.Use(test.UriLogger);
        } 
    }
}
