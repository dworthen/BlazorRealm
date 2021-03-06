﻿using Microsoft.Extensions.DependencyInjection;

namespace Blazor.Realm.ReduxDevTools.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddRealmReduxDevToolServices(this IServiceCollection services)
        {
            services.AddScoped<ReduxDevToolsInterop>();
            return services;
        }
    }
}
