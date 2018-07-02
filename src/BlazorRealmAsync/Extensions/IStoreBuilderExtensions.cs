using Blazor.Realm.Extensions;

namespace Blazor.Realm.Async.Extensions
{
    public static class IStoreBuilderExtensions
    {
        public static IStoreBuilder<TState> UseRealmAsync<TState>(this IStoreBuilder<TState> builder)
        {
            return builder.UseMiddleware<TState, HandleAsyncActions<TState>>();
        }
    }
}
