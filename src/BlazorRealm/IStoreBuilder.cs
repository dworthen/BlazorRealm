using System;
using System.Collections.Generic;

namespace Blazor.Realm
{
    public interface IStoreBuilder<TState>
    {
        IServiceProvider ServiceProvider { get; set; }
        List<Func<Dispatcher<TState>, Dispatcher<TState>>> Middleware { get; set; }

        IStoreBuilder<TState> Build();
    }
}
