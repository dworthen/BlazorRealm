﻿using Microsoft.AspNetCore.Blazor.Browser.Services;
using System;

namespace Blazor.Realm.ReduxDevTools
{
    public static class Extensions
    {
        public static void UseRealmReduxDevTools<TState>(this IStoreBuilder<TState> builder)
        {
            builder.UseMiddleware<TState, HandleReduxDevTools<TState>>(builder.ServiceProvider);
        }

        public static void UseRealmReduxDevTools<TState>(this IStoreBuilder<TState> builder, Type[] actionsToIgnore)
        {
            builder.UseMiddleware<TState, HandleReduxDevTools<TState>>(builder.ServiceProvider, actionsToIgnore);
        }
    }
}