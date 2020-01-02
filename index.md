---
_disableAffix: true
uid: index-page
---

# Blazor Realm

[Redux](https://redux.js.org/) state management for [Blazor.net](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor).

### Centralized state.

Application state is defined in one location. This helps reason about state and acts as a single source of truth.

### Single direction data flow.

Instead of updating the application state directly, views dispatch actions for a Realm store to handle. The store, in turn, updates the centralized state and triggers a rerender of the UI.

### View as a function of data.

The single direction data flow pattern works well with UI frameworks that render UI, or views, as functions of data. The component UI model of Blazor works in this way. Components receive data as props and render based on the provided values. Components rerender as data/props change.

### Middleware

Realm is extendable and supports the same middleware design pattern as Redux.

### Integrates with Redux DevTools

The [Blazor WebAssembly](https://docs.microsoft.com/en-us/aspnet/core/blazor/hosting-models?view=aspnetcore-3.1#blazor-webassembly) project supports integrating with [Redux DevTools](https://github.com/reduxjs/redux-devtools) for a powerful debugging experience with time-traveling capabilities.

> **NOTE**
>
> The Redux DevTools middleware does not currently support [Blazor Server](https://docs.microsoft.com/en-us/aspnet/core/blazor/hosting-models?view=aspnetcore-3.1#blazor-server). Work is being done to support integrating with Redux DevTools from a Blazor Server project.

### Getting Started

[Quickstart](./docs/quickstart.md)
