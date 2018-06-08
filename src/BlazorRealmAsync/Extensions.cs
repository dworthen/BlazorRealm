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
        public static void UseRealmAsync<TState>(this RealmMiddlewareBuilder<TState> builder)
        {
            builder.Use(HandleAsyncActions.Handle);
        }
    }
}
