using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blazor.Realm.ReduxDevTools
{
    public class ReduxDevToolsInterop
    {
        protected readonly string JSNameSpace = "BlazorRealmReduxDevTools";
        protected readonly IJSRuntime JSRuntime;
        protected readonly IUriHelper UriHelper;
        public static event EventHandler<Message> MessageReceived;

        public ReduxDevToolsInterop(IJSRuntime jsRuntime, IUriHelper uriHelper)
        {
            JSRuntime = jsRuntime;
            UriHelper = uriHelper;
        }

        [JSInvokable]
        public static void OnMessageReceived(Message message) => MessageReceived?.Invoke(null, message);
        public async Task<bool> IsAvailableAsync()
        {
            bool result = await JSRuntime.InvokeAsync<bool>($"{JSNameSpace}.IsAvailable");
            return result;
        }
        public void Connect()
        {
            JSRuntime.InvokeAsync<object>($"{JSNameSpace}.Connect");
        }
        public void Init(object state)
        {
            JSRuntime.InvokeAsync<object>($"{JSNameSpace}.Init", state);
        }
        public void Subscribe()
        {
            JSRuntime.InvokeAsync<object>($"{JSNameSpace}.Subscribe");
        }
        public void Send(object action, object state)
        {
            JSRuntime.InvokeAsync<object>($"{JSNameSpace}.Send", new { type = action.GetType().FullName }, state);
        }
        public void UnSubscribe()
        {
            JSRuntime.InvokeAsync<object>($"{JSNameSpace}.UnSubscribe");
        }
        public void Disconnect()
        {
            JSRuntime.InvokeAsync<object>($"{JSNameSpace}.Disconnect");
        }
    }
}
