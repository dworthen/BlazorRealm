# Blazor Realm

Redux state management for Blazor. Realm takes the design patterns of Redux, centralized state and single direction data flow, and brings them to Blazor.

### Centralized state. 

Application state is defined in one location. This not only helps reason about an application's data needs but also acts as a single source of truth, eliminating state conflicts.

### Single direction data flow.

Instead of updating an application's state directly, the UI dispatches actions for the Realm store to handle, in turn, updating the centralized application state and triggering a rerender of the UI.

### View as a function of data. 

The single direction data flow pattern works well with UI frameworks that render UI, or views, as functions of data. The component UI model of Blazor works in this way. Components receive data as props and render based on the provided values. Components rerender as data/props update, receiving the updated values for rendering.

# [AppState.cs](#tab/AppState)

```csharp
public class AppState
{
    public int Count { get; set; }
}
```

# [Actions.cs](#tab/Actions)

```csharp
public class IncrementByValue : IRealmAction
{
    public int Value { get; set; }
    public IncrementByValue(int value)
    {
        Value = value;
    }
}
```

# [Reducer.cs](#tab/Reducers)

```csharp
public static class Reducers
{
    public static AppState RootReducer(AppState appState, IRealmAction action)
    {
        if(appState == null)
        {
            throw new ArgumentNullException(nameof(appState));
        }

        // Return a new AppState
        return new AppState
        {
            // Composing Reducers for components
            Count = CounterReducer(appState.Count, action)
	      }
    }

    public static int CounterReducer(int count, IRealmAction action)
    {
        switch(action)
        {
            case IncrementByValue a:
                return count + a.Value;
            default:
                return count;
        }
    }
}
```

# [Startup.cs](#tab/Startup)

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddRealmStore<AppState>(new AppState(), Reducers.RootReducer);
}
```

---