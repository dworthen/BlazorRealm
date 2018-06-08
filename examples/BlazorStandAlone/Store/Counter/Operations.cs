using Blazor.Realm;
using Blazor.Realm.Async;
using BlazorStandAlone.Models;
using System;
using System.Threading.Tasks;
using static BlazorStandAlone.Store.Counter.Actions;

namespace BlazorStandAlone.Store.Counter
{
    public class Operations
    {
        public class AsyncIncrementAction : IAsyncAction
        {
            public Store<AppState> Store { get; set; }
            public int IncrementAmount { get; set; }
            public AsyncIncrementAction(Store<AppState> store, int incrementAmount = 1)
            {
                Store = store ?? throw new ArgumentNullException(nameof(store));
                IncrementAmount = incrementAmount;
            }
            public async Task Invoke()
            {
                Store.Dispatch(new StartLoading());
                await Task.Delay(3000);
                Store.Dispatch(new IncrementByValue(IncrementAmount));
                Store.Dispatch(new EndLoading());
            }
        }

    }
}
