using System;
using System.Collections.Generic;
using static BlazorRealm.Delegates;

namespace BlazorRealm
{
    public class RealmMiddlewareBuilder<TState>
    {
        private readonly List<Func<Store<TState>, Dispatcher, Dispatcher>> _middleware = new List<Func<Store<TState>, Dispatcher, Dispatcher>>();
        private readonly Store<TState> _store;
        public Dispatcher Dispatch { get; private set; }

        public RealmMiddlewareBuilder(Store<TState> store)
        {
            _store = store;
            Dispatch = store.Dispatch;
        }

        public void Build()
        {
            _middleware.Reverse();
            foreach (var middleware in _middleware)
            {
                Dispatch = middleware(_store, Dispatch);
            }
        }

        public void Use(Func<Store<TState>, Dispatcher, Dispatcher> middleware)
        {
            _middleware.Add(middleware);
        }
    }
}
