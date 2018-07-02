namespace Blazor.Realm.Async
{
    public class HandleAsyncActions<TState>
    {

        private readonly Store<TState> _store;
        private readonly Dispatcher<TState> _next;

        public HandleAsyncActions(Store<TState> store, Dispatcher<TState> next)
        {
            _store = store;
            _next = next;
        }

        public TState Invoke(IRealmAction action)
        {
            if (action is IAsyncRealmAction)
            {
                action.GetType().GetMethod("Invoke").Invoke(action, null);
                return default(TState);
            }
            else
            {
                return _next(action);
            }
        }

    }
}
