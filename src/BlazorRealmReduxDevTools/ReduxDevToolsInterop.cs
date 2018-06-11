using System;
using Microsoft.AspNetCore.Blazor.Browser.Interop;

namespace Blazor.Realm.ReduxDevTools
{
    public static class ReduxDevToolsInterop
    {
        public static event EventHandler<string> MessageReceived;
        public static void OnMessageReceived(string message) => MessageReceived?.Invoke(null, message);
        public static bool IsAvailable()
        {
            return RegisteredFunction.Invoke<bool>("Blazor.Realm.ReduxDevTools.IsAvailable");
        }
        public static void Connect()
        {
            RegisteredFunction.Invoke<object>("Blazor.Realm.ReduxDevTools.Connect");
        }
        public static void Init(object state)
        {
            RegisteredFunction.Invoke<object>("Blazor.Realm.ReduxDevTools.Init", state);
        }
        public static void Subscribe()
        {
            RegisteredFunction.Invoke<object>("Blazor.Realm.ReduxDevTools.Subscribe");
        }
        public static void Send(object action, object state)
        {
            RegisteredFunction.Invoke<object>("Blazor.Realm.ReduxDevTools.Send", action, state);
        }
        public static void UnSubscribe()
        {
            RegisteredFunction.Invoke<object>("Blazor.Realm.ReduxDevTools.UnSubscribe");
        }
        public static void Disconnect()
        {
            RegisteredFunction.Invoke<object>("Blazor.Realm.ReduxDevTools.Disconnect");
        }
    }
}
