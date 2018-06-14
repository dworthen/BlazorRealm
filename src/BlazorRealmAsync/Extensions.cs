using Blazor.Realm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Realm.Async
{
    public static class Extensions
    {
        public static IRealmStoreBuilder<TState> UseRealmAsync<TState>(this IRealmStoreBuilder<TState> builder)
        {
            return builder.UseMiddleware<TState, HandleAsyncActions<TState>>();
        }
    }
}
