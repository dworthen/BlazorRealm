using Blazor.Realm.Extensions;
using System;
using System.Collections.Generic;

namespace Blazor.Realm
{
    public class StoreBuilder<TState> : IStoreBuilder<TState>
    {
        public IServiceProvider ServiceProvider { get; set; }
        public List<Func<Dispatcher<TState>, Dispatcher<TState>>> Middleware { get; set; }

        public StoreBuilder(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            Middleware = new List<Func<Dispatcher<TState>, Dispatcher<TState>>>();
        }

        public IStoreBuilder<TState> Build()
        {
            Dispatcher<TState> dispatcher = this.GetInitialStoreDispatch<TState>();
            for (int i = Middleware.Count - 1; i >= 0; i--)
            {
                dispatcher = Middleware[i](dispatcher);
            }
            this.SetStoreDispatch<TState>(dispatcher);
            return this;
        }
    }
}
