using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Http.Extensions;
using System.Linq;
using Microsoft.AspNetCore.Components.Routing;
using System.Text.Json;

namespace Blazor.Realm.ReduxDevTools
{
    public class HandleReduxDevTools<TState>
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly NavigationManager UriHelper;
        private readonly List<Tuple<string, string>> History = new List<Tuple<string, string>>();
        private readonly Type[] ActionsToIgnore;
        private readonly Store<TState> Store;
        private readonly Dispatcher<TState> Next;
        private readonly ReduxDevToolsInterop reduxDevToolsInterop;
        private bool Configured = false;

        public HandleReduxDevTools(Store<TState> store, Dispatcher<TState> next, IServiceProvider serviceProvider) : this(store, next, serviceProvider, new Type[] { })
        {

        }

        public HandleReduxDevTools(Store<TState> store, Dispatcher<TState> next, IServiceProvider serviceProvider, Type[] actionsToIgnore)
        {
            Store = store;
            Next = next;
            ServiceProvider = serviceProvider;
            ActionsToIgnore = actionsToIgnore ?? new Type[] { };
            UriHelper = ServiceProvider.GetService<NavigationManager>();
            //IJSRuntime JSRuntime = ServiceProvider.GetService<IJSRuntime>();
            //reduxDevToolsInterop = new ReduxDevToolsInterop(JSRuntime);
            reduxDevToolsInterop = ServiceProvider.GetService<ReduxDevToolsInterop>();
            TState state = Store.GetState();
            History.Add(new Tuple<string, string>(UriHelper.Uri, JsonSerializer.Serialize<TState>(state)));

            reduxDevToolsInterop.Connect();
            reduxDevToolsInterop.Init(state);

            UriHelper.LocationChanged += (object s, LocationChangedEventArgs uri) =>
            {
                Dictionary<string, StringValues> queryParams = QueryHelpers.ParseQuery(new Uri(UriHelper.Uri).Query);
                if (!queryParams.ContainsKey("RealmReduxDevToolsNavigation"))
                {
                    TState State = Store.GetState();
                    History.Add(new Tuple<string, string>(UriHelper.Uri, JsonSerializer.Serialize<TState>(state)));
                    reduxDevToolsInterop.Send(new RealmReduxDevToolsUriChanged(), State);
                }
            };

            ReduxDevToolsInterop.MessageReceived += (object sender, Message eventArgs) =>
            {
                if (eventArgs.Payload?.Type == "JUMP_TO_STATE" || eventArgs.Payload?.Type == "JUMP_TO_ACTION")
                {
                    int index = eventArgs.Payload.Type == "JUMP_TO_STATE"
                        ? eventArgs.Payload.Index
                        : eventArgs.Payload.ActionId;
                    Tuple<string, string> desiredState = History[index];
                    Store.Dispatch(new RealmReduxDevToolsAppState<TState>(JsonSerializer.Deserialize<TState>(desiredState.Item2)));
                    if (desiredState.Item1 != UriHelper.Uri)
                    {
                        UriBuilder uri = new UriBuilder(desiredState.Item1);
                        //Dictionary<string, StringValues> query = QueryHelpers.ParseNullableQuery(uri.Query);
                        //query.Add("RealmReduxDevToolsNavigation", "true");
                        //uri.Query = QueryHelpers.
                        List<KeyValuePair<string, string>> query = QueryHelpers
                            .ParseQuery(uri.Query)
                            .SelectMany(x => x.Value, (col, value) => new KeyValuePair<string, string>(col.Key, value))
                            .ToList();
                        QueryBuilder qb = new QueryBuilder(query);
                        qb.Add("RealmReduxDevToolsNavigation", "true");

                        //string newUri = QueryHelpers.AddQueryString(desiredState.Item1, "RealmReduxDevToolsNavigation", "true");
                        uri.Query = qb.ToQueryString().Value;
                        string newUri = uri.ToString();

                        UriHelper.NavigateTo(newUri);
                    }
                }
            };
            reduxDevToolsInterop.Subscribe();
        }

        public TState Invoke(IRealmAction action)
        {
            if(!Configured)
            {
                
                
                Configured = true;
            }
            switch(action)
            {
                case RealmReduxDevToolsAppState<TState> a:
                    return a.State;
                default:
                    TState nextState = Next(action);
                    if (nextState != null && Array.IndexOf(ActionsToIgnore, action.GetType()) == -1)
                    {
                        History.Add(new Tuple<string, string>(UriHelper.Uri, JsonSerializer.Serialize<TState>(nextState)));
                        reduxDevToolsInterop.Send(action, nextState);
                    }
                    return nextState;
            }
        }
        
    }

    class RealmReduxDevToolsAppState<TState> : IRealmAction
    {
        public TState State { get; set; }

        public RealmReduxDevToolsAppState(TState state)
        {
            State = state;
        }
    }

    class RealmReduxDevToolsUriChanged : IRealmAction
    {
    }
}
