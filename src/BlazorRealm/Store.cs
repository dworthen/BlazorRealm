using Microsoft.JSInterop;
using System;

namespace Blazor.Realm
{
    public class Store<TState>
    {
        private TState State;
        private readonly Reducer<TState> _rootReducer;

        internal Dispatcher<TState> _dispatch;

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

        public TState GetState()
        {
            try
            {
                return Json.Deserialize<TState>(Json.Serialize(State));
            } catch (Exception e)
            {
                return State;
            }
        }

        internal TState InitialDispatch(IRealmAction action)
        {
            TState localState = _rootReducer(State, action);
            return localState;
        }

        public void Dispatch(IRealmAction action)
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
