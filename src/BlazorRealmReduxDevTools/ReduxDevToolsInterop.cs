using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Blazor.Realm.ReduxDevTools
{
    public static class ReduxDevToolsInterop
    {
        private static readonly string JSNameSpace = "BlazorRealmReduxDevTools";
        public static event EventHandler<MessageEventArgs> MessageReceived;
        [JSInvokable]
        public static void OnMessageReceived(string message) => MessageReceived?.Invoke(null, new MessageEventArgs(message));
        public static async Task<bool> IsAvailableAsync()
        {
            bool result = await JSRuntime.Current.InvokeAsync<bool>($"{JSNameSpace}.IsAvailable");
            return result;
        }
        public static void Connect()
        {
            JSRuntime.Current.InvokeAsync<object>($"{JSNameSpace}.Connect");
        }
        public static void Init(object state)
        {
            JSRuntime.Current.InvokeAsync<object>($"{JSNameSpace}.Init", state);
        }
        public static void Subscribe()
        {
            JSRuntime.Current.InvokeAsync<object>($"{JSNameSpace}.Subscribe");
        }
        public static void Send(object action, object state)
        {
            JSRuntime.Current.InvokeAsync<object>($"{JSNameSpace}.Send", new { type = action.GetType().Name }, state);
        }
        public static void UnSubscribe()
        {
            JSRuntime.Current.InvokeAsync<object>($"{JSNameSpace}.UnSubscribe");
        }
        public static void Disconnect()
        {
            JSRuntime.Current.InvokeAsync<object>($"{JSNameSpace}.Disconnect");
        }
    }
}
