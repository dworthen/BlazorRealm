using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Realm.Extensions
{
    public static class IStoreBuilderExtensions
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

        public static IStoreBuilder<TState> Use<TState>(this IStoreBuilder<TState> builder, Func<Dispatcher<TState>, Dispatcher<TState>> middleware)
        {
            builder.Middleware?.Add(middleware);
            return builder.Build();
        }

        public static IStoreBuilder<TState> Use<TState>(this IStoreBuilder<TState> builder, Func<Store<TState>, Dispatcher<TState>, Dispatcher<TState>> middleware)
        {
            return builder.Use((Dispatcher<TState> next) =>
            {
                Store<TState> store = builder.ServiceProvider.GetService(typeof(Store<TState>)) as Store<TState>;
                return middleware(store, next);
            });
        }
    }
}
