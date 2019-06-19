using Microsoft.Extensions.DependencyInjection;
using System;

namespace Blazor.Realm.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static Store<TState> AddRealmStore<TState>(this IServiceCollection services, TState initialState, Reducer<TState> rootReducer)
        {
            Store<TState> store = new Store<TState>(initialState, rootReducer);
            services.AddSingleton<Store<TState>>(store);
            //IServiceProvider serviceProvider = services.BuildServiceProvider();
            //IStoreBuilder<TState> builder = new StoreBuilder<TState>(serviceProvider);
            services.AddSingleton<IStoreBuilder<TState>, StoreBuilder<TState>>(/*builder*/);
            return store;
        }
    }
}
