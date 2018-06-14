using Microsoft.AspNetCore.Blazor.Components;
using System;

namespace Blazor.Realm
{
    public class RealmComponent<TState> : BlazorComponent, IDisposable
    {
        [Inject]
        public Store<TState> Store { get; set; }

        public TState State => Store.State;
        public Action<IAction> Dispatch => Store.Dispatch;

        public virtual void Dispose()
        {
            Store.Change -= OnChangeHandler;
        }

        protected override void OnInit()
        {
            Store.Change += OnChangeHandler;
        }

        protected virtual void OnChangeHandler(object sender, EventArgs e)
        {
            StateHasChanged();
        }
    }
}
