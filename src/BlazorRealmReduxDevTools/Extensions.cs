using Microsoft.AspNetCore.Blazor.Browser.Services;
using System;

namespace Blazor.Realm.ReduxDevTools
{
    public static class Extensions
    {
        public static void UseRealmReduxDevTools<TState>(this RealmMiddlewareBuilder<TState> builder, BrowserServiceProvider serviceProvider, Type[] actionsToIgnore = null)
        {
            HandleReduxDevTools<TState> devTools = new HandleReduxDevTools<TState>(serviceProvider, actionsToIgnore);
            builder.Use(devTools.Handle);
        }
    }
}
