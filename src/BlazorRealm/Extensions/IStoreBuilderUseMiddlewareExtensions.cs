using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace Blazor.Realm.Extensions
{
    public static class IStoreBuilderUseMiddlewareExtensions
    {
        internal const string InvokeMethodName = "Invoke";

        public static IStoreBuilder<TState> UseMiddleware<TState, TMiddleware>(this IStoreBuilder<TState> builder, params object[] args)
        {
            return builder.UseMiddleware(typeof(TMiddleware), args);
        }

        public static IStoreBuilder<TState> UseMiddleware<TState>(this IStoreBuilder<TState> builder, Type middleware, params object[] args)
        {

            IServiceProvider serviceProvider = builder.ServiceProvider;
            Store<TState> store = builder.ServiceProvider.GetService(typeof(Store<TState>)) as Store<TState>;

            return builder.Use((Dispatcher<TState> next) =>
            {
                MethodInfo[] methods = middleware.GetMethods(BindingFlags.Instance | BindingFlags.Public);
                MethodInfo[] invokeMethods = methods.Where(m =>
                    string.Equals(m.Name, InvokeMethodName, StringComparison.Ordinal)
                    ).ToArray();

                if (invokeMethods.Length != 1)
                {
                    throw new InvalidOperationException("Middleware should have only one Invoke method.");
                }

                MethodInfo methodinfo = invokeMethods[0];
                if (!typeof(TState).IsAssignableFrom(methodinfo.ReturnType))
                {
                    throw new InvalidOperationException($"Middleware should return type {typeof(TState)}");
                }

                ParameterInfo[] parameters = methodinfo.GetParameters();
                if (parameters.Length != 1 || parameters[0].ParameterType != typeof(IRealmAction))
                {
                    throw new InvalidOperationException("Middleware Invoke should accept one argument of type IRealmAction");
                }

                object[] ctorArgs = new object[args.Length + 2];
                ctorArgs[0] = store;
                ctorArgs[1] = next;
                Array.Copy(args, 0, ctorArgs, 2, args.Length);
                var instance = ActivatorUtilities.CreateInstance(serviceProvider, middleware, ctorArgs);
                //if (parameters.Length == 1)
                //{
                    return (Dispatcher<TState>)methodinfo.CreateDelegate(typeof(Dispatcher<TState>), instance);
                //}
                
            });

        }
    }
}
