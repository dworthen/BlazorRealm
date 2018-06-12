using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Browser.Services;
using Microsoft.AspNetCore.Blazor.Services;
using System;
using System.Collections.Generic;

namespace Blazor.Realm.ReduxDevTools
{
    public class HandleReduxDevTools<TState>
    {
        private BrowserServiceProvider ServiceProvider { get; set; }
        private IUriHelper UriHelper { get; set; }

        private List<Tuple<string, string>> History { get; set; } = new List<Tuple<string, string>>();
        private Type[] ActionsToIgnore { get; set; }

        public HandleReduxDevTools(BrowserServiceProvider serviceProvider, Type[] actionsToIgnore = null)
        {
            ServiceProvider = serviceProvider;
            ActionsToIgnore = actionsToIgnore;
            UriHelper = ServiceProvider.GetService(typeof(IUriHelper)) as IUriHelper;
            Store<TState> store = ServiceProvider.GetService(typeof(Store<TState>)) as Store<TState>;
            History.Add(new Tuple<string, string>(UriHelper.GetAbsoluteUri(), JsonUtil.Serialize(store.State)));

            ReduxDevToolsInterop.Connect();
            ReduxDevToolsInterop.Init(store.State);
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

        public Dispatcher<TState> Handle(Store<TState> store, Dispatcher<TState> next)
        {
            return (IAction action) =>
            {
                switch(action)
                {
                    case RealmReduxDevToolsAppState<TState> a:
                        return a.State;
                    default:
                        TState nextState = next(action);
                        if (nextState != null && (ActionsToIgnore == null || Array.IndexOf(ActionsToIgnore, action.GetType()) == -1))
                        {
                            History.Add(new Tuple<string, string>(UriHelper.GetAbsoluteUri(), JsonUtil.Serialize(nextState)));
                            ReduxDevToolsInterop.Send(action, nextState);
                        }
                        return nextState;
                }
            };
        }
        
    }

    class Init: IAction { }

    class RealmReduxDevToolsAppState<TState> : IAction
    {
        public TState State { get; set; }

        public RealmReduxDevToolsAppState(TState state)
        {
            State = state;
        }
    }
}
