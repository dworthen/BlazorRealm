using System;
using static BlazorRealm.Delegates;

namespace BlazorRealm
{
    public class Store<TState>
    {
        private readonly Reducer<TState> _rootReducer;
        private readonly RealmMiddlewareBuilder<TState> _builder;

        public TState State { get; private set; }
        public Dispatcher Dispatch { get; private set; }
        public event EventHandler Change;

        //TODO: Implement IBuilder interface and pass in Builder to decouple.
        public Store(TState initialState, Reducer<TState> rootReducer)
        {
            State = initialState;
            _rootReducer = rootReducer;
            Dispatch = InitialDispatch;
            _builder = new RealmMiddlewareBuilder<TState>(this);
        }

        public void OnChange(EventArgs e)
        {
            Change?.Invoke(this, e);
        }

        public void InitialDispatch(IAction action)
        {
            State = _rootReducer(State, action);
            OnChange(null);
        }

        public void ApplyMiddleWare(Action<RealmMiddlewareBuilder<TState>> builder = null)
        {
            builder?.Invoke(_builder);
            _builder.Build();
            Dispatch = _builder.Dispatch;
        }
    }
}
