using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Realm
{
    public class RealmStoreBuilder<TState> : IRealmStoreBuilder<TState>
    {
        public IServiceProvider ServiceProvider { get; set; }
        public List<Func<Dispatcher<TState>, Dispatcher<TState>>> Middleware { get; set; }

        public RealmStoreBuilder(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            Middleware = new List<Func<Dispatcher<TState>, Dispatcher<TState>>>();
        }
    }
}
