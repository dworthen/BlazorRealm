using System;
using System.Collections.Generic;

namespace Blazor.Realm
{
    public class RealmMiddlewareBuilder<TState>
    {
        private readonly List<Func<Store<TState>, Dispatcher<TState>, Dispatcher<TState>>> _middleware = new List<Func<Store<TState>, Dispatcher<TState>, Dispatcher<TState>>>();
        private readonly Store<TState> _store;
        internal Dispatcher<TState> Dispatch { get; set; }

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

        public void Use(Func<Store<TState>, Dispatcher<TState>, Dispatcher<TState>> middleware)
        {
            _middleware.Add(middleware);
        }
    }
}
