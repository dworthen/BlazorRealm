using static Blazor.Realm.Delegates;

namespace Blazor.Realm.Async
{
    public static class HandleAsyncActions
    {
        public static Dispatcher Handle<TState>(Store<TState> store, Dispatcher next)
        {
            return (IAction action) =>
            {
                if (action is IAsyncAction)
                {
                    action.GetType().GetMethod("Invoke").Invoke(action, null);
                } else
                {
                    next(action);
                }
            };
        }

    }
}
