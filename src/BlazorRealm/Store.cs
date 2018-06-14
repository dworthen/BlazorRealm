using System;

namespace Blazor.Realm
{
    public class Store<TState>
    {
        private readonly Reducer<TState> _rootReducer;
        internal Dispatcher<TState> _dispatch;

        public TState State { get; private set; }
        public event EventHandler Change;

        //TODO: Implement IBuilder interface and pass in Builder to decouple.
        public Store(TState initialState, Reducer<TState> rootReducer)
        {
            State = initialState;
            _rootReducer = rootReducer;
            _dispatch = InitialDispatch;
        }

        public void OnChange(EventArgs e)
        {
            Change?.Invoke(this, e);
        }

        internal TState InitialDispatch(IAction action)
        {
            TState localState = _rootReducer(State, action);
            return localState;
        }

        public void Dispatch(IAction action)
        {
            TState localState = _dispatch(action);
            if (localState != null)
            {
                State = localState;
                OnChange(null);
            }
        }
    }
}
