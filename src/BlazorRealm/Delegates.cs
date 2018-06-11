namespace Blazor.Realm
{
    public delegate TState Dispatcher<TState>(IAction action);
    public delegate TState Reducer<TState>(TState previousState, IAction action);
}
