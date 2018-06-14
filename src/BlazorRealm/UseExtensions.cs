using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Realm
{
    public static class UseExtensions
    {
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
