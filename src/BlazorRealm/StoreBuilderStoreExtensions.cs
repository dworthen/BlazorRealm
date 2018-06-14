using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Realm
{
    public static class StoreBuilderStoreExtensions
    {
        public static Dispatcher<TState> GetInitialStoreDispatch<TState>(this IStoreBuilder<TState> builder)
        {
            Store<TState> store = builder.ServiceProvider.GetService(typeof(Store<TState>)) as Store<TState>;
            return store.InitialDispatch;
        }

        public static void SetStoreDispatch<TState>(this IStoreBuilder<TState> builder, Dispatcher<TState> dispatcher)
        {
            Store<TState> store = builder.ServiceProvider.GetService(typeof(Store<TState>)) as Store<TState>;
            store._dispatch = dispatcher;
        }
    }
}
