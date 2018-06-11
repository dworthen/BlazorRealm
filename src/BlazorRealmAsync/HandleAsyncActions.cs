using Blazor.Realm;

namespace Blazor.Realm.Async
{
    public static class HandleAsyncActions
    {
        public static Dispatcher<TState> Handle<TState>(Store<TState> store, Dispatcher<TState> next)
        {
            return (IAction action) =>
            {
                if (action is IAsyncAction)
                {
                    action.GetType().GetMethod("Invoke").Invoke(action, null);
                    return default(TState);
                } else
                {
                    return next(action);
                }
            };
        }

    }
}
