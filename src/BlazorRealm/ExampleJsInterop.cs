using System;
using Microsoft.AspNetCore.Blazor.Browser.Interop;

namespace Blazor.Realm
{
    public class ExampleJsInterop
    {
        public static string Prompt(string message)
        {
            return RegisteredFunction.Invoke<string>(
                "BlazorRealm.ExampleJsInterop.Prompt",
                message);
        }
    }
}
