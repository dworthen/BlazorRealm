using Blazor.Realm.Extensions;
using System;

namespace Blazor.Realm.ReduxDevTools.Extensions
{
    public static class IStoreBuilderExtensions
    {
        public static void UseRealmReduxDevTools<TState>(this IStoreBuilder<TState> builder)
        {
            builder.UseMiddleware<TState, HandleReduxDevTools<TState>>(builder.ServiceProvider);
        }

        public static void UseRealmReduxDevTools<TState>(this IStoreBuilder<TState> builder, IServiceProvider serviceProvider)
        {
            builder.UseMiddleware<TState, HandleReduxDevTools<TState>>(serviceProvider);
        }

        public static void UseRealmReduxDevTools<TState>(this IStoreBuilder<TState> builder, Type[] actionsToIgnore)
        {
            builder.UseMiddleware<TState, HandleReduxDevTools<TState>>(builder.ServiceProvider, actionsToIgnore);
        }

        public static void UseRealmReduxDevTools<TState>(this IStoreBuilder<TState> builder, IServiceProvider serviceProvider, Type[] actionsToIgnore)
        {
            builder.UseMiddleware<TState, HandleReduxDevTools<TState>>(serviceProvider, actionsToIgnore);
        }
    }
}
