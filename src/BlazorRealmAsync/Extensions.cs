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
        public static IStoreBuilder<TState> UseRealmAsync<TState>(this IStoreBuilder<TState> builder)
        {
            return builder.UseMiddleware<TState, HandleAsyncActions<TState>>();
        }
    }
}
