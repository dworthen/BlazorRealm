using Microsoft.AspNetCore.Blazor.Components;
using System;

namespace Blazor.Realm
{
    public class RealmComponent<TState> : BlazorComponent, IDisposable
    {
        [Inject]
        public Store<TState> Store { get; private set; }

        public TState State { get; private set; }
        public Action<IRealmAction> Dispatch => Store.Dispatch;

        public virtual void Dispose()
        {
            Store.Change -= OnChangeHandler;
        }

        protected override void OnInit()
        {
            State = Store.GetState();
            Store.Change += OnChangeHandler;
        }

        protected virtual void OnChangeHandler(object sender, EventArgs e)
        {
            State = Store.GetState();
            StateHasChanged();
        }
    }
}
