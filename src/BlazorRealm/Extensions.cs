using Microsoft.Extensions.DependencyInjection;
using static BlazorRealm.Delegates;

namespace BlazorRealm
{
    public static class Extensions
    {
        public static Store<TState> AddRealmStore<TState>(this IServiceCollection services, TState initialState, Reducer<TState> rootReducer)
        {
            Store<TState> store = new Store<TState>(initialState, rootReducer);
            services.AddSingleton<Store<TState>>(store);
            return store;
        }
    }
}
