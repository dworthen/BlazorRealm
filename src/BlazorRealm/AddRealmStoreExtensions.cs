using Microsoft.Extensions.DependencyInjection;
using System;

namespace Blazor.Realm
{
    public static class AddRealmStoreExtensions
    {
        public static Store<TState> AddRealmStore<TState>(this IServiceCollection services, TState initialState, Reducer<TState> rootReducer)
        {
            Store<TState> store = new Store<TState>(initialState, rootReducer);
            services.AddSingleton<Store<TState>>(store);
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            IRealmStoreBuilder<TState> builder = new RealmStoreBuilder<TState>(serviceProvider);
            services.AddSingleton<IRealmStoreBuilder<TState>>(builder);
            return store;
        }
    }
}
