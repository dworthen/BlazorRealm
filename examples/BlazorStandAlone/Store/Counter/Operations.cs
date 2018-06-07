using BlazorRealm;
using BlazorRealmAsync;
using BlazorStandAlone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorStandAlone.Store.Counter
{
    public class AsyncIncrementAction : IAsyncAction
    {
        public Store<AppState> Store { get; set; }
        public int IncrementAmount { get; set; }
        public AsyncIncrementAction(Store<AppState> store, int incrementAmount = 1)
        {
            Store = store;
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
