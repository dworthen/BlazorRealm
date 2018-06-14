using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Realm
{
    public interface IStoreBuilder<TState>
    {
        IServiceProvider ServiceProvider { get; set; }
        List<Func<Dispatcher<TState>, Dispatcher<TState>>> Middleware { get; set; }

        IStoreBuilder<TState> Build();
    }
}
