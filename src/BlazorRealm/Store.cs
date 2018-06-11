using System;

namespace Blazor.Realm
{
    public class Store<TState>
    {
        private readonly Reducer<TState> _rootReducer;
        private readonly RealmMiddlewareBuilder<TState> _builder;

        public TState State { get; private set; }
        public Dispatcher<TState> Dispatch { get; private set; }
        public event EventHandler Change;

        //TODO: Implement IBuilder interface and pass in Builder to decouple.
        public Store(TState initialState, Reducer<TState> rootReducer)
        {
            State = initialState;
            _rootReducer = rootReducer;
            Dispatch = InitialDispatch;
            _builder = new RealmMiddlewareBuilder<TState>(this);
            Dispatch = (IAction action) =>
            {
                TState localState = _builder.Dispatch(action);
                if (localState != null)
                {
                    State = localState;
                }
                OnChange(null);
                return State;
            };
        }

        public void OnChange(EventArgs e)
        {
            Change?.Invoke(this, e);
        }

        private TState InitialDispatch(IAction action)
        {
            TState localState = _rootReducer(State, action);
            //OnChange(null);
            return localState;
        }

        public void ApplyMiddleWare(Action<RealmMiddlewareBuilder<TState>> builder = null)
        {
            builder?.Invoke(_builder);
            _builder.Build();
            //Dispatch = _builder.Dispatch;
        }
    }
}
