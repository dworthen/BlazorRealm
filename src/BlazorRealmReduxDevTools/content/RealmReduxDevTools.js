var assemblyName = "Blazor.Realm.ReduxDevTools";
var namespace = "Blazor.Realm.ReduxDevTools";

var devTools = window.__REDUX_DEVTOOLS_EXTENSION__;

Blazor.registerFunction(namespace + '.IsAvailable', function () {
    return devTools !== undefined;
});

Blazor.registerFunction(namespace + '.Connect', function () {
    devTools = devTools.connect();
});

Blazor.registerFunction(namespace + '.Init', function (state, subscribe) {
    return devTools.init(state);
});

Blazor.registerFunction(namespace + '.Send', function (action, state) {
    return devTools.send(action, state);
});

Blazor.registerFunction(namespace + '.Subscribe', function () {
    const messageReceivedMethod = Blazor.platform.findMethod(
        assemblyName,
        namespace,
        "ReduxDevToolsInterop",
        "OnMessageReceived"
    );

    devTools.subscribe((message) => {
        var json = JSON.stringify(message);
        //console.log(json);
        //console.log(JSON.parse(json));
    //    Blazor.invokeDotNetMethod({
    //        type: {
    //            aseembly: assemblyName,
    //            name: namespace + '.ReduxDevToolsInterop'
    //        },
    //        method: {
    //            name: 'OnMessageReceived'
    //        }
    //    }, json);
        Blazor.platform.callMethod(messageReceivedMethod, null, [Blazor.platform.toDotNetString(json)]);
    });
});

Blazor.registerFunction(namespace + '.UnSubscribe', function () {
    devTools.unsubscribe();
});

Blazor.registerFunction(namespace + '.Disconnect', function () {
    devTools.disconnect();
});