using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Browser.Services;
using Microsoft.AspNetCore.Blazor.Services;
using System;
using System.Collections.Generic;

namespace Blazor.Realm.ReduxDevTools
{
    public class HandleReduxDevTools<TState>
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IUriHelper UriHelper;
        private readonly List<Tuple<string, string>> History = new List<Tuple<string, string>>();
        private readonly Type[] ActionsToIgnore;
        private readonly Store<TState> Store;
        private readonly Dispatcher<TState> Next;

        public HandleReduxDevTools(Store<TState> store, Dispatcher<TState> next, IServiceProvider serviceProvider) : this(store, next, serviceProvider, new Type[] { })
        {

        }

        public HandleReduxDevTools(Store<TState> store, Dispatcher<TState> next, IServiceProvider serviceProvider, Type[] actionsToIgnore)
        {
            Store = store;
            Next = next;
            ServiceProvider = serviceProvider;
            ActionsToIgnore = actionsToIgnore ?? new Type[] { };
            UriHelper = ServiceProvider.GetService(typeof(IUriHelper)) as IUriHelper;
            TState state = store.GetState();
            History.Add(new Tuple<string, string>(UriHelper.GetAbsoluteUri(), JsonUtil.Serialize(state)));

            ReduxDevToolsInterop.Connect();
            ReduxDevToolsInterop.Init(state);
            ReduxDevToolsInterop.MessageReceived += (object sender, MessageEventArgs eventArgs) =>
            {
                if(eventArgs.Message.Payload?.Type == "JUMP_TO_STATE" || eventArgs.Message.Payload?.Type == "JUMP_TO_ACTION")
                {
                    int index = eventArgs.Message.Payload.Type == "JUMP_TO_STATE"
                        ? eventArgs.Message.Payload.Index
                        : eventArgs.Message.Payload.ActionId;
                    Tuple<string, string> desiredState = History[index];
                    store.Dispatch(new RealmReduxDevToolsAppState<TState>(JsonUtil.Deserialize<TState>(desiredState.Item2)));
                    if(desiredState.Item1 != UriHelper.GetAbsoluteUri())
                    {
                        UriHelper.NavigateTo(desiredState.Item1);
                    }
                }
            };
            ReduxDevToolsInterop.Subscribe();
        }

        public TState Invoke(IAction action)
        {
            switch(action)
            {
                case RealmReduxDevToolsAppState<TState> a:
                    return a.State;
                default:
                    TState nextState = Next(action);
                    if (nextState != null && Array.IndexOf(ActionsToIgnore, action.GetType()) == -1)
                    {
                        History.Add(new Tuple<string, string>(UriHelper.GetAbsoluteUri(), JsonUtil.Serialize(nextState)));
                        ReduxDevToolsInterop.Send(action, nextState);
                    }
                    return nextState;
            }
        }
        
    }

    class RealmReduxDevToolsAppState<TState> : IAction
    {
        public TState State { get; set; }

        public RealmReduxDevToolsAppState(TState state)
        {
            State = state;
        }
    }
}
