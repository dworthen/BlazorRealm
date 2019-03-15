# Blazor Realm

Redux state management for [blazor.net](https://blazor.net).

[Documentation](https://dworthen.github.io/BlazorRealm/../quickstart.html)

> **NOTE FROM BLAZOR**
>
> Blazor is an unsupported experimental web framework that shouldn't be used for production workloads at this time.

- [Blazor Realm](#blazor-realm)
- [Getting Started](#getting-started)
- [Application State](#application-state)
- [Actions](#actions)
- [Reducer](#reducer)
- [Add Realm Service](#add-realm-service)
- [State Components](#state-components)
- [Async Actions](#async-actions)
  - [Register Async Middleware](#register-async-middleware)
  - [Dispatching Async Actions](#dispatching-async-actions)
- [Redux Dev Tools](#redux-dev-tools)
  - [Register DevTools Middleware](#register-devtools-middleware)
  - [Ignoring Specific Actions](#ignoring-specific-actions)

# Getting Started

1.  For getting started with Blazor, visit https://blazor.net/../get-started.html.
2.  Install https://www.nuget.org/packages/Blazor.Realm/.

**Compatibility**

| Blazor Realm | Blazor |
| ------------ | ------ |
| 0.5.9        | 0.5.x  |
| 0.6.x        | 0.6.x  |
| 0.7.x        | 0.7.x  |


# Application State

```csharp
// AppState.cs
public class AppState
{
    public int Count { get; set; }
    public IEnumerable<WeatherForecast> WeatherForecasts { get; set; } = new WeatherForecast[] { };
}

public class WeatherForecast
{
    public DateTime Date { get; set; }
    public int TemperatureC { get; set; }
    public int TemperatureF { get; set; }
    public string Summary { get; set; }
}
```

# Actions

```csharp
//Actions.cs

// Counter Actions
public class IncrementByOne : IRealmAction { }

public class IncrementByValue : IRealmAction
{
    public int Value { get; set; }
    public IncrementByValue(int value)
    {
        Value = value;
    }
}

public class DecrementByOne : IRealmAction { }

public class DecrementByValue : IRealmAction
{
    public int Value { get; set; }
    public DecrementByValue(int value)
    {
        Value = value;
    }
}

public class ResetCount : IRealmAction { }

// WeatherForecasts Actions
public class ClearWeatherForecasts : IRealmAction { }

public class SetWeatherForecasts : IRealmAction
{
    public IEnumerable<WeatherForecast> WeatherForecasts { get; set; }
    public SetWeatherForecasts(IEnumerable<WeatherForecast> forecasts)
    {
        WeatherForecasts = forecasts;
    }
}
```

Actions must implement `IRealmAction`.

# Reducer

```csharp
// Reducer.cs

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
            Count = CounterReducer(appState.Count, action),
            WeatherForecasts = WeatherForecastsReducer(appState.WeatherForecasts, action)
        };
    }

    public static int CounterReducer(int count, IRealmAction action)
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
    WeatherForecastsReducer(IEnumerable<WeatherForecast> forecasts, IRealmAction action)
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

# Add Realm Service

```csharp
// Startup.cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddRealmStore<AppState>(new AppState(), Reducers.RootReducer);
}
```

# State Components

```csharp
@page "/counter"
@addTagHelper *, Blazor.Realm
@using Blazor.Realm;

<RealmStateContainer TState="AppState" Context="store">
    <ComponentTemplate>
        @{
            AppState State = store.GetState();
            Action<IRealmAction> Dispatch = store.Dispatch;

            Action IncrementCount = () => Dispatch(new IncrementByValue(ChangeAmount));
            Action DecrementCount = () => Dispatch(new DecrementByValue(ChangeAmount));
        }
        <h1>Counter</h1>

        <p>Current count: @State.Count</p>

        Change By: <input type="text" name="IncementAmount" bind="@ChangeAmount" /><br />

        <button class="btn btn-primary" onclick="@IncrementCount">Increment</button><br />
        <button class="btn btn-primary" onclick="@DecrementCount">Decrement</button><br />
    </ComponentTemplate>
</RealmStateContainer>

@functions {
    private int ChangeAmount { get; set; } = 1;
}
```

# Async Actions

As with Redux, Async actions in Realm are handled by middleware. Download the [Blazor.Realm.Async nuget package](https://www.nuget.org/packages/Blazor.Realm.Async/).

I prefer placing async actions in a seperate _Operations.cs_ file.

```csharp
// Operations.cs

public class AsyncIncrementCounter : IAsyncRealmAction
{
    public Store<AppState> Store { get; set; }
    public int IncrementAmount { get; set; }
    public AsyncIncrementCounter(Store<AppState> store, int incrementAmount = 1)
    {
        Store = store ?? throw new ArgumentNullException(nameof(store));
        IncrementAmount = incrementAmount;
    }

    public async Task Invoke()
    {
        // Dispatch events to handle the start, middle
        // and end of async actions.
        // StartLoading and Endloading
        // are normal, sync actions defined in Actions.cs
        Store.Dispatch(new StartLoading());
        // Something async, like network call
        await Task.Delay(3000);
        // Call sync action to increment Counter
        Store.Dispatch(new IncrementByValue(IncrementAmount));
        Store.Dispatch(new EndLoading());
    }
}
```

Async actions must implement the `IAsyncRealmAction` interface and, in turn, implement `Task Invoke` method.

## Register Async Middleware

```csharp
// Startup.cs
public void Configure(IBlazorApplicationBuilder app, 
    IStoreBuilder<AppState> RealmStoreBuilder)
{
    RealmStoreBuilder.UseRealmAsync<AppState>();

    app.AddComponent<App>("app");
}
```

## Dispatching Async Actions

In _Counter.cshtml_

```csharp
@page "/counter"
@addTagHelper *, Blazor.Realm
@using Blazor.Realm;

<RealmStateContainer TState="AppState" Context="store">
    <ComponentTemplate>
        @{
            AppState State = store.GetState();
            Action<IRealmAction> Dispatch = store.Dispatch;

            Action IncrementCountAsync = () => Dispatch(new AsyncIncrementCounter(store, ChangeAmount));
            Action DecrementCount = () => Dispatch(new DecrementByValue(ChangeAmount));
        }
        <h1>Counter</h1>

        <p>Current count: @State.Count</p>

        Change By: <input type="text" name="IncementAmount" bind="@ChangeAmount" /><br />

        <button class="btn btn-primary" onclick="@IncrementCountAsync">Increment Async</button><br />
        <button class="btn btn-primary" onclick="@DecrementCount">Decrement</button><br />
    </ComponentTemplate>
</RealmStateContainer>

@functions {
    private int ChangeAmount { get; set; } = 1;
}
```

# Redux Dev Tools

![Redux DevTools](../images/redux-devtools.GIF)

Connecting to the [Redux DevTools](http://extension.remotedev.io/) browser extension is handled by middleware.

Steps for connecting to Redux Dev Tools:

1.  [Install the browser extension](http://extension.remotedev.io/#installation).
2.  Install the middleware, https://www.nuget.org/packages/Blazor.Realm.ReduxDevTools/

## Register DevTools Middleware

```csharp
// Startup.cs
public void Configure(IBlazorApplicationBuilder app, 
    IStoreBuilder<AppState> RealmStoreBuilder)
{
    RealmStoreBuilder.UseRealmAsync<AppState>();

    RealmStoreBuilder.UseRealmReduxDevTools<AppState>();

    app.AddComponent<App>("app");
}
```

> **NOTE**
>
> The order in which middleware is registred matters. Add `UseRealmReduxDevTools` after `UseRealmAsync`.

## Ignoring Specific Actions

```csharp
// Startup.cs
public void Configure(IBlazorApplicationBuilder app, 
    IStoreBuilder<AppState> RealmStoreBuilder)
{
    RealmStoreBuilder.UseRealmAsync<AppState>();

    RealmStoreBuilder.UseRealmReduxDevTools<AppState>(new System.Type[]
    {
        typeof(Actions.Counter.Dispose)
    });

    app.AddComponent<App>("app");
}
```
