using BlazorRealm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BlazorRealm.Delegates;

namespace BlazorRealmAsync
{
    public static class HandleAsyncActions
    {
        public static Dispatcher Handle<TState>(Store<TState> store, Dispatcher next)
        {
            return (IAction action) =>
            {
                if (action is IAsyncAction)
                {
                    action.GetType().GetMethod("Invoke").Invoke(action, null);
                } else
                {
                    next(action);
                }
            };
        }

    }
}
