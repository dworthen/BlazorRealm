---
_disableAffix: true
uid: index-page
---

# Blazor Realm

Redux state management for Blazor. Realm takes the design patterns of Redux, centralized state and single direction data flow, and brings them to Blazor.

### Centralized state. 

Application state is defined in one location. This helps reason about an application's data needs and acts as a single source of truth, eliminating state conflicts.

### Single direction data flow.

Instead of updating the application state directly, the UI dispatches actions for the Realm store to handle. The store, in turn, updates the centralized state and triggers a rerender of the UI.

### View as a function of data. 

The single direction data flow pattern works well with UI frameworks that render UI, or views, as a function of data. The component UI model of Blazor works in this way. Components receive data as props and render based on the provided values. Components rerender as data/props change, receiving the updated values for rendering.

