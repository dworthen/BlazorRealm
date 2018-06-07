﻿using Microsoft.AspNetCore.Blazor.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BlazorRealm.Delegates;

namespace BlazorRealm
{
    public class RealmComponent<TState> : BlazorComponent, IDisposable
    {
        [Inject]
        public Store<TState> Store { get; set; }

        public TState State => Store.State;
        public Dispatcher Dispatch => Store.Dispatch;

        public virtual void Dispose()
        {
            Store.Change -= OnChangeHandler;
        }

        protected override void OnInit()
        {
            Store.Change += OnChangeHandler;
        }

        public virtual void OnChangeHandler(object sender, EventArgs e)
        {
            StateHasChanged();
        }
    }
}