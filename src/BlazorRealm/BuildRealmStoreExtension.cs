namespace Blazor.Realm
{
    public static class BuildRealmStoreExtension
    {
        public static IRealmStoreBuilder<TState> BuildRealmStore<TState>(this IRealmStoreBuilder<TState> builder)
        {
            Store<TState> store = builder.ServiceProvider.GetService(typeof(Store<TState>)) as Store<TState>;
            Dispatcher<TState> dispatcher = store.InitialDispatch;
            for (int i = builder.Middleware.Count - 1; i>= 0; i--)
            {
                dispatcher = builder.Middleware[i](dispatcher);
            }
            store._dispatch = dispatcher;
            return builder;
        }
    }
}
