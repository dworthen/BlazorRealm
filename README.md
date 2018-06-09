# Blazor Realm

Redux inspired state management for [blazor.net](https://blazor.net).

> **NOTE FROM BLAZOR**
> 
> Blazor is an unsupported experimental web framework that shouldn't be used for production workloads at this time.

# Getting Started

1. For getting started with Blazor, visit https://blazor.net/docs/get-started.html.
2. Install https://www.nuget.org/packages/Blazor.Realm/. 
3. Add `@using Blazor.Realm;` to *_ViewImports.cshtml*.
4. Add the `RealmStore` service in *program.cs*
    ```C#
    static void Main(string[] args)
    {
        var serviceProvider = new BrowserServiceProvider(services =>
        {
            // Add any custom services here
            services.AddRealmStore<AppState>(new AppState(), Store.RootReducer.Reduce);
        });

        new BrowserRenderer(serviceProvider).AddComponent<App>("app");
    }
    ```
    Don't forget to add `using Blazor.Realm;`


> **NOTE**
> 
> Blazor Realm is compatible with Blazor 0.4.0.

# Application State

```C#
// AppState.cs
public class AppState
{
    public int Count { get; set; }
    public IEnumerable<WeatherForecast> WeatherForecasts { get; set; } = new WeatherForecast[] { };
}
```

# Actions

```C#
//Actions.cs

// Counter Actions
public class IncrementByOne : IAction { }

public class IncrementByValue : IAction
{
    public int Value { get; set; }
    public IncrementByValue(int value)
    {
        Value = value;
    }
}

public class DecrementByOne : IAction { }

public class DecrementByValue : IAction
{
    public int Value { get; set; }
    public DecrementByValue(int value)
    {
        Value = value;
    }
}

public class ResetCount : IAction { }

// WeatherForecasts Actions
public class ClearWeatherForecasts : IAction { }

public class SetWeatherForecasts : IAction
{
    public IEnumerable<WeatherForecast> WeatherForecasts { get; set; }
    public SetWeatherForecasts(IEnumerable<WeatherForecast> forecasts)
    {
        WeatherForecasts = forecasts;
    }
}
```

Actions must implement `IAction`. Don't forget `using Blazor.Realm`.

# Reducer

```C#
// Reducer.cs
public static class RootReducer
{
    public static AppState RootReducer(AppState appState, IAction action)
    {
        if(appState == null)
        {
            throw new ArgumentNullException(nameof(appState));
        }

        // Return a new AppState
        return new AppState
        {
            // Composing Reducers for components
            Count = CounterReducer(appState.Count, action),
            WeatherForecasts = WeatherForecastsReducer(appState.WeatherForecasts, action)
        };
    }

    public static int CounterReducer(int count, IAction action)
    {
        switch(action)
        {
            case IncrementByOne _:
                return count + 1;
            case IncrementByValue a:
                return count + a.Value;
            case DecrementByOne _:
                return count - 1;
            case DecrementByValue a:
                return count - a.Value;
            case ResetCount _:
                return 0;
            default:
                return count;
        }
    }

    public static IEnumerable<WeatherForecast> 
    WeatherForecastsReducer(IEnumerable<WeatherForecast> forecasts, IAction action)
    {
        switch(action)
        {
            case ClearWeatherForecasts _:
                return new WeatherForecast[] { };
            case SetWeatherForecasts a:
                return a.WeatherForecasts;
            default:
                return forecasts;
        }
    }

}
```

# Blazor Components

Don't forget to add `using Blazor.Realm;` to each component. Alternatively, add `using Blazor.Realm;` 
to *_ViewImports.cshtml*

```C#
@page "/counter"
@* 1. Inject Store<AppState> from services *@
@inject Store<AppState> store
@* 2. Implement IDisposable *@
@implements IDisposable
@* Or add to _ViewImports.cshtml *@
@using Blazor.Realm;

<h1>Counter</h1>

<p>Current count: @State.Count</p>

Change By: <input type="text" name="IncementAmoung" bind="@ChangeAmount" /><br />

<button class="btn btn-primary" onclick="@IncrementCount">Increment</button><br />
<button class="btn btn-primary" onclick="@DecrementCount">Decrement</button><br />


@functions {
    private int ChangeAmount { get; set; } = 1;

    // 3. Handlers for Store and State from injected services
    private Store<AppState> Store => store;
    private AppState State => Store.State;

    void IncrementCount()
    {
        Store.Dispatch(new IncrementByValue(ChangeAmount));
    }

    void DecrementCount()
    {
        Store.Dispatch(new DecrementByValue(ChangeAmount));
    }

    protected override void OnInit()
    {
        // 4. Register handler for Realm Store Change event
        Store.Change += OnChangeHandler;
    }

    protected void OnChangeHandler(object sender, EventArgs e)
    {
        // 5. Inform Blazor that the state has changed
        // due to changes in the Realm Store.
        StateHasChanged();
    }

    public virtual void Dispose()
    {
        // 6. Remove event handlers when component is disposed
        Store.Change -= OnChangeHandler;
    }
}

```

The basic pattern (boilerplate)

1. Inject `Store<AppState>`.
2. Register event handler for `Blazor.Realm.Store`'s `change` event `OnInit`.
3. Remove event handlers when the component is `Dispose`ed.
4. `Dispatch` actions and read state from `Store.State`.

The first three items 