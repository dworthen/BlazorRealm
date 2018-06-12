# Blazor Realm

Redux inspired state management for [blazor.net](https://blazor.net).

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

Actions must implement `IAction`. Don't forget to add `using Blazor.Realm;`.

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

Don't forget to add `using Blazor.Realm;`.

# Register the App Store as a Service

```C#
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

Change By: <input type="text" name="IncementAmount" bind="@ChangeAmount" /><br />

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

## Component Pattern (boilerplate)

1.  Inject `Store<AppState>`.
2.  Register event handler for `Blazor.Realm.Store`'s `change` event in `OnInit`.
3.  Remove event handlers in the component's `Dispose`.
4.  `Dispatch` actions and read state from `Store.State`.

The first three items are repeated in all components connecting a Realm store. Instead of repeating this pattern, components may inherit from `Blazor.Realm.RealmComponent`.

```C#
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

```C#
// Program.cs

public class Program
{
    static void Main(string[] args)
    {
        Store<AppState> store = null;
        var serviceProvider = new BrowserServiceProvider(services =>
        {
            // Add any custom services here
            store = services.AddRealmStore<AppState>(new AppState(), Store.RootReducer.Reduce);
        });

        store.ApplyMiddleWare(builder =>
        {
            // Example middleware
            builder.Use((Store<AppState> localStore, Dispatcher next) =>
            {
                return (IAction action) =>
                {
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
                    Store<AppState> nextState = next(action);
                    // do more stuff, like more logging
                    Console.WriteLine("Action {0} complete.", action.GetType().Name);
                    Console.WriteLine("New state: {0}",
                        JsonUtil.Serialize(nextState));

                    return nextState;
                };
            });

        });

        new BrowserRenderer(serviceProvider).AddComponent<App>("app");
    }
}
```

## Middleware as Extension Methods

```C#
// Logger.cs
public static class Logger
{
    public static Dispatcher<TState> Log<TState>(Store<TState> store, Dispatcher next)
    {
        return (IAction action) =>
        {
            // Logger
        };
    }
}

// Extensions.cs
public static class Extensions
{
    public static void UseLogger<TState>(this RealmMiddlewareBuilder<TState> builder)
    {
        builder.Use(Logger.Log);
    }
}

// Program.cs

store.ApplyMiddleWare(builder =>
{
    builder.UseLogger<AppState>();
});
```

# Async Actions

As with Redux, Async actions in Realm are handled by middleware. Download the [Blazor.Realm.Async nuget package](https://www.nuget.org/packages/Blazor.Realm.Async/).

Following a [ducks](https://medium.freecodecamp.org/scaling-your-redux-app-with-ducks-6115955638be) organizational structure, I place async actions in a seperate _Operations.cs_ file.

```C#
// Operations.cs

public class AsyncIncrementCounter : IAsyncAction
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

Async actions must implement the `IAsyncAction` interface and, in turn, implement `Task Invoke` method.

## Dispatching Async Actions

In _Counter.cshtml_

```C#
@page "/counter"
@inherits RealmComponent<AppState>

...

<button class="btn btn-primary" onclick="@AsyncIncrement">Async Increment</button><br />

@functions {
    ...

    void AsyncIncrement() // this method is not really async
    {
        // No need to await async action
        Dispatch(new AsyncIncrementAction(Store, Delta));
    }

    ...
}
```

# Redux Dev Tools

Connecting to the [Redux DevTools](http://extension.remotedev.io/) browser extension is handled by middleware.

Steps for connecting to Redux Dev Tools:

1. [Install the browser extension](http://extension.remotedev.io/#installation).
2. Install the middleware, https://www.nuget.org/packages/Blazor.Realm.ReduxDevTools/

Using the middleware

```C#
// program.cs
Store<AppState> store = null;
var serviceProvider = new BrowserServiceProvider(services =>
{
    // Add any custom services here
    store = services.AddRealmStore<AppState>(new AppState(), Store.RootReducer.Reduce);
});

store.ApplyMiddleWare(builder =>
{
    builder.UseRealmAsync<AppState>();

    builder.UseRealmReduxDevTools<AppState>(serviceProvider);
});

new BrowserRenderer(serviceProvider).AddComponent<App>("app");
```

> **NOTE**
>
> The order in which middleware is registred matters. Add `builder.UseRealmReduxDevTools` after `builder.UseRealmAsync`.

## Ignoring Specific Actions in Redux Dev Tools

```C#
// program.cs
store.ApplyMiddleWare(builder =>
{
    builder.UseRealmAsync<AppState>();

    builder.UseRealmReduxDevTools<AppState>(serviceProvider, new Type[] {
        // ResetCount actions will not show up in the Redux DevTools 
        // browser extension
        typeof(ResetCount)
    });
});
```