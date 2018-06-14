namespace Blazor.Realm
{
    public delegate TState Dispatcher<TState>(IRealmAction action);
    public delegate TState Reducer<TState>(TState previousState, IRealmAction action);
}
