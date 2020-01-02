# Blazor Realm

[Redux](https://redux.js.org/) state management for [Blazor.net](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor).

# Getting Started

1.  For getting started with Blazor, visit https://docs.microsoft.com/en-us/aspnet/core/blazor/get-started?view=aspnetcore-3.1&tabs=visual-studio.
2.  Install https://www.nuget.org/packages/Blazor.Realm/.

# Application State

```csharp
// AppState.cs
public class AppState
{
    public int Count { get; set; } = 0;
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

Register the Realm store service in the Startup.cs `ConfigureServices` method.

```csharp
// Startup.cs
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddRealmStore<AppState>(new AppState(), Reducers.RootReducer);
}
```

# Components

There are two ways to work with a Realm store within a Blazor component.

1. State components
2. RealmComponent Inheritance

> **Note**
>
> Don't forget to add `@using Blazor.Realm` to the top of Razor components or to the \_imports.razor file.

## State Component

The `RealmStateContainer` component is akin to the render prop technique common in React components. Like the render prop technique, `RealmStateContainer`

1. Follows a component model for working with and injecting state into child components.
2. Will render the `ComponentTemplate` instead of implementing/rendering its own logic.

`RealmStateContainer` will dynamically render the `ComponentTemplate` and inject the `AppState` that was registered in Startup.cs `ConfigureServices`.

Here is an example.

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

@code {
    private int ChangeAmount { get; set; } = 1;
}
```

- `TState` attribute specifies the type of the Realm Store to inject. This type should match what was registered in the Startup.cs `ConfigureServices` method.
- `Context` attribute defines the variable name used for exposing the Realm store to the `ComponentTemplate`.
- `ComponentTemplate` specify what to render and has access to the Realm store captured in the context variable that was injected by `RealmStateContainer`.

Here is another example: https://github.com/dworthen/BlazorRealm/blob/master/examples/BlazorClientApp/Pages/FetchData.razor

Notice that this example does not render content directly. Instead the `ComponentTemplate` renders a custom `WeatherForecastsTemplate` component, setting all necessary props (https://github.com/dworthen/BlazorRealm/blob/master/examples/BlazorClientApp/Pages/FetchData.razor#L16). This is similar to defining pure UI components in React as functional components and then wrapping those components in Class components for managing and injecting state. That way, the pure components stay pure and reusable.

## RealmComponent Inheritance

The second option for working with a Realm store it to inherit from `RealmComponent<TState>`. This will expose

1. The application state as `State`.
2. A `Dispatch` action.

```csharp
// Counter.razor
@page "/counter"
@inherits RealmComponent<AppState>

<CounterTemplate Count=@State.Count ChangeAmount=1 OnIncrement=@Increment OnDecrement=@Decrement>

@code {
    void Increment()
    {
        Dispatch(new Redux.Actions.Counter.IncrementByValue(ChangeAmount));
    }

    void Decrement()
    {
        Dispatch(new Redux.Actions.Counter.DecrementByValue(ChangeAmount));
    }
}

// CounterTemplate.razor
<h1>Counter</h1>

<p>Current count: @Count</p>

Change By: <input type="text" bind="@ChangeAmount" /><br />

<button class="btn btn-primary" onclick="@OnIncrement">Increment</button><br />
<button class="btn btn-primary" onclick="@OnDecrement">Decrement</button><br />

@code {
  [Parameter] public int Count { get; set; } = 0;
  [Parameter] public int ChangeAmount { get; set; } = 1;
  [Parameter] public Action OnIncrement { get; set; }
  [Parameter] public Action OnDecrement { get; set; }
}
```

The above example uses two components. `Counter` inherits from `RealmComponent` and is thus coupled to the store/data source. `CounterTemplate`, on the other hand, is a pure component receiving all data as props and therefore more reusable. This is similar to defining pure UI components in React as functional components and then wrapping those components in Class components for managing and injecting state.

# Async Actions

As with Redux, Async actions in Realm are handled by middleware.

1. Download the [Blazor.Realm.Async nuget package](https://www.nuget.org/packages/Blazor.Realm.Async/).
2. Add Async actions, this time implementing `IAsyncRealmAction`.

```csharp
// Actions.cs

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

```csharp
// Counter.razor
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

Connecting to [Redux DevTools](http://extension.remotedev.io/) is handled by middleware.

Steps for connecting to Redux Dev Tools:

1.  [Install the browser extension](http://extension.remotedev.io/#installation).
2.  Install the middleware, https://www.nuget.org/packages/Blazor.Realm.ReduxDevTools/

> **NOTE**
>
> The Redux DevTools middleware does not currently support [Blazor Server](https://docs.microsoft.com/en-us/aspnet/core/blazor/hosting-models?view=aspnetcore-3.1#blazor-server). Work is being done to support integrating with Redux DevTools from a Blazor Server project.

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
