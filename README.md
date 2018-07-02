# Blazor Realm

Redux state management for [blazor.net](https://blazor.net).

> **NOTE FROM BLAZOR**
>
> Blazor is an unsupported experimental web framework that shouldn't be used for production workloads at this time.

- [Blazor Realm](#blazor-realm)
- [Getting Started](#getting-started)
- [Application State](#application-state)
- [Actions](#actions)
- [Reducer](#reducer)
- [Register the App Store as a Service](#register-the-app-store-as-a-service)
- [Blazor Components](#blazor-components)
  - [Component Pattern (boilerplate)](#component-pattern-boilerplate)
- [Middleware](#middleware)
  - [Middleware as Extension Methods](#middleware-as-extension-methods)
- [Async Actions](#async-actions)
  - [Adding Async Middleware](#adding-async-middleware)
  - [Dispatching Async Actions](#dispatching-async-actions)
- [Redux Dev Tools](#redux-dev-tools)
  - [Ignoring Specific Actions in Redux Dev Tools](#ignoring-specific-actions-in-redux-dev-tools)

# Getting Started

1.  For getting started with Blazor, visit https://blazor.net/docs/get-started.html.
2.  Install https://www.nuget.org/packages/Blazor.Realm/.

> **NOTE**
>
> Blazor Realm is compatible with Blazor 0.4.0.

# Application State

```csharp
// AppState.cs
public class AppState
{
    public int Count { get; set; }
    public IEnumerable<WeatherForecast> WeatherForecasts { get; set; } = new WeatherForecast[] { };
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

Actions must implement `IRealmAction`. Don't forget to add `using Blazor.Realm;`.

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

Don't forget to add `using Blazor.Realm;`.

# Register the App Store as a Service

```csharp
static void Main(string[] args)
{
    var serviceProvider = new BrowserServiceProvider(services =>
    {
        // Add any custom services here
        services.AddRealmStore<AppState>(new AppState(), RootReducer);
    });

    new BrowserRenderer(serviceProvider).AddComponent<App>("app");
}
```

Don't forget to add `using Blazor.Realm;`.

# Blazor Components

Don't forget to add `using Blazor.Realm;` to each component. Alternatively, add `using Blazor.Realm;`
to _\_ViewImports.cshtml_

```csharp
@page "/counter"
@inject Store<AppState> store
@implements IDisposable

@* Or add to _ViewImports.cshtml *@
@using Blazor.Realm;

<h1>Counter</h1>

<p>Current count: @State.Count</p>

Change By: <input type="text" name="IncementAmount" bind="@ChangeAmount" /><br />

<button class="btn btn-primary" onclick="@IncrementCount">Increment</button><br />
<button class="btn btn-primary" onclick="@DecrementCount">Decrement</button><br />


@functions {
    private int ChangeAmount { get; set; } = 1;

    // Handlers for Store and State from injected services
    private Store<AppState> Store => store;
    private AppState State;

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
        // Retrieve State from the store
        // Store.GetState returns a clone of State
        // as long as State is serializable.
        // This makes it ok to bind to @State in local
        // components as it will not mutate central state.
        // Only dispatching actions will update central state.
        State = Store.GetState();
        // Register handler for Realm Store Change event
        Store.Change += OnChangeHandler;
    }

    protected void OnChangeHandler(object sender, EventArgs e)
    {
        // Retreive new state on changes
        // and inform Blazor that state has changed 
        // for rerender.
        State = Store.GetState();
        StateHasChanged();
    }

    public virtual void Dispose()
    {
        // Remove event handlers when component is disposed
        Store.Change -= OnChangeHandler;
    }
}
```

## Component Pattern (boilerplate)

1.  Inject `Store<AppState>`.
2.  Store a local copy of state, `State = Store.GetState()`.
3.  Register event handler for the change event in `OnInit` and Update local state.
4.  Remove event handlers in `Dispose`.
5.  `Dispatch` actions and read state from `State`.

The first 4 items are repeated in all components connecting to a Realm store. Instead of repeating this pattern, components may inherit from `Blazor.Realm.RealmComponent`.

```csharp
@page "/counter"
@inherits RealmComponent<AppState>
@* Or add to _ViewImports.cshtml *@
@using Blazor.Realm;

<h1>Counter</h1>

<p>Current count: @State.Count</p>

Change By: <input type="text" name="IncementAmount" bind="@ChangeAmount"/><br />

<button class="btn btn-primary" onclick="@IncrementCount">Increment</button><br />
<button class="btn btn-primary" onclick="@DecrementCount">Decrement</button><br />


@functions {
    private int ChangeAmount { get; set; } = 1;

    /**
    * Inheritiing from RealmComponent exposes Store, State and Dispatch.
    */

    void IncrementCount()
    {
        Dispatch(new IncrementByValue(ChangeAmount));
    }

    void DecrementCount()
    {
        Dispatch(new DecrementByValue(ChangeAmount));
    }

    // May still override OnInit, OnChangeHandler or Dispose
    // Just need to call the base method
    public override void Dispose()
    {
        base.Dispose();
        // Example of resetting local state when
        // app navigates away from page.
        Dispatch(new ResetCount());
    }
}
```

# Middleware

```csharp
// Program.cs

public class Program
{
    static void Main(string[] args)
    {
        var serviceProvider = new BrowserServiceProvider(services =>
        {
            // Add any custom services here
            services.AddRealmStore<AppState>(new AppState(), RootReducer);
        });

        // using Microsoft.Extensions.DependencyInjection;
        IStoreBuilder<AppState> storeBuilder = 
            ServiceProvider.GetService<IStoreBuilder<AppState>>();

        storeBuilder.Use((Store<AppState> localStore, Dispatcher<AppState> next) =>
        {
            return (IRealmAction action) =>
            {
                /**
                * localStore = Store singleton. This allows one to dispatch
                * actions down the whole middleware pipeline. Example: error
                * handling middleware. Try passing actions down pipeline but if exception
                * is caught then dispatch a new ErrorAction down the pipeline for the
                * application to handle.
                *
                * next = next middleware in the pipeline.  next(action) will continue
                * sending the current action down the middleware pipeline.
                *
                * action = current action dispatched. 
                *
                * returns TState = Application State.
                */

                // Do stuff, like logging
                Console.WriteLine("Dispatching {0}", action.GetType().Name);
                Console.WriteLine("Old state: {0}",
                    JsonUtil.Serialize(localStore.State));

                // May also dispatch new actions.
                // This will send the action through the
                // whole middleware pipeline,
                // not just the next one in the pipeline.
                localStore.Dispatch(new SomeAction());

                // send current action to the next middleware
                AppState nextState = next(action);
                // do more stuff, like more logging
                Console.WriteLine("Action {0} complete.", action.GetType().Name);
                Console.WriteLine("New state: {0}",
                    JsonUtil.Serialize(nextState));

                return nextState;
            };
        });


        new BrowserRenderer(serviceProvider).AddComponent<App>("app");
    }
}
```

## Middleware as Extension Methods

```csharp
// Logger.cs
public class Logger<TState>
{
    private readonly Store<TState> _store;
    private readonly Dispatcher<TState> _next;

    public Logger(Store<TState> store, Dispatcher<TSate> next) 
    {
        _store = store;
        _next = next;
    }

    public TState Invoke(IRealmAction action)
    {
        // Logger implementation...
        return _next(action);
    }
}

// Extensions.cs
// Optional
public static class Extensions
{
    public static IStoreBuilder UseLogger<TState>(this IStoreBuilder<TState> builder)
    {
        return builder.UseMiddleware<TState, Logger<TState>>();
    }
}

// Program.cs

...

// using Microsoft.Extensions.DependencyInjection;
IStoreBuilder<AppState> storeBuilder = 
    ServiceProvider.GetService<IStoreBuilder<AppState>>();

storeBuilder.UseLogger<AppState>();
// Or without using an extension
// storeBuilder.UseMiddleware<AppState, Logger<AppState>>();

new BrowserRenderer(serviceProvider).AddComponent<App>("app");

...

```

# Async Actions

As with Redux, Async actions in Realm are handled by middleware. Download the [Blazor.Realm.Async nuget package](https://www.nuget.org/packages/Blazor.Realm.Async/).

Following a [ducks](https://medium.freecodecamp.org/scaling-your-redux-app-with-ducks-6115955638be) organizational structure, I place async actions in a seperate _Operations.cs_ file.

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

## Adding Async Middleware

```csharp
// program.cs

var serviceProvider = new BrowserServiceProvider(services =>
{
    // Add any custom services here
    services.AddRealmStore<AppState>(new AppState(), RootReducer);
});

// using Microsoft.Extensions.DependencyInjection;
IStoreBuilder<AppState> storeBuilder = 
    ServiceProvider.GetService<IStoreBuilder<AppState>>();

storeBuilder.UseRealmAsync<AppState>();

new BrowserRenderer(serviceProvider).AddComponent<App>("app");


```

## Dispatching Async Actions

In _Counter.cshtml_

```csharp
@page "/counter"
@inherits RealmComponent<AppState>

...

<button class="btn btn-primary" onclick="@AsyncIncrement">Async Increment</button><br />

@functions {
    ...

    void AsyncIncrement() // this method is not really async
    {
        Dispatch(new AsyncIncrementAction(Store, ChangeAmount));
    }

    ...
}
```

# Redux Dev Tools

![Redux DevTools](docs/images/redux-devtools.GIF)

Connecting to the [Redux DevTools](http://extension.remotedev.io/) browser extension is handled by middleware.

Steps for connecting to Redux Dev Tools:

1. [Install the browser extension](http://extension.remotedev.io/#installation).
2. Install the middleware, https://www.nuget.org/packages/Blazor.Realm.ReduxDevTools/

Using the middleware

```csharp
// program.cs
var serviceProvider = new BrowserServiceProvider(services =>
{
    // Add any custom services here
    services.AddRealmStore<AppState>(new AppState(), RootReducer);
});

// using Microsoft.Extensions.DependencyInjection;
IStoreBuilder<AppState> storeBuilder = 
    ServiceProvider.GetService<IStoreBuilder<AppState>>();

storeBuilder.UseRealmAsync<AppState>();

storeBuilder.UseRealmReduxDevTools<AppState>();

new BrowserRenderer(serviceProvider).AddComponent<App>("app");
```

> **NOTE**
>
> The order in which middleware is registred matters. Add `UseRealmReduxDevTools` after `UseRealmAsync`.

## Ignoring Specific Actions in Redux Dev Tools

```csharp
// program.cs

storeBuilder.UseRealmReduxDevTools<AppState>(new Type[] {
    // ResetCount actions will not show up in the Redux DevTools 
    // browser extension
    typeof(ResetCount)
});

```