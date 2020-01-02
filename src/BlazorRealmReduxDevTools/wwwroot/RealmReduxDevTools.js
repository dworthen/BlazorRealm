window.BlazorRealmReduxDevTools = (function () {
    var assemblyName = "Blazor.Realm.ReduxDevTools";
    var namespace = "Blazor.Realm.ReduxDevTools";

    var devTools = window.__REDUX_DEVTOOLS_EXTENSION__;

    return {
        IsAvailable: function () {
            return devTools !== undefined;
        },

        Connect: function () {
            devTools({
                latency: 0
            });
            devTools = devTools.connect();
        },

        Init: function (state, subscribe) {
            return devTools.init(state);
        },

        Send: function (action, state) {
            return devTools.send(action, state);
        },

        Subscribe: function () {
            devTools.subscribe((message) => {
                //var json = JSON.stringify(message);
                DotNet.invokeMethod(assemblyName, "OnMessageReceived", message);
            });
        },

        Unsubscribe: function () {
            devTools.unsubscribe();
        },

        Disconnect: function () {
            devTools.disconnect();
        }
    }

    
})();
