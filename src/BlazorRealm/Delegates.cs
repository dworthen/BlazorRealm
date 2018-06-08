namespace Blazor.Realm
{
    public class Delegates
    {
        public delegate void Dispatcher(IAction action);
        public delegate TState Reducer<TState>(TState previousState, IAction action);
    }
}
