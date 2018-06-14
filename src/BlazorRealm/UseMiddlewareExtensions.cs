using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace Blazor.Realm
{
    public static class UseMiddlewareExtensions
    {
        internal const string InvokeMethodName = "Invoke";

        //private static readonly MethodInfo GetServiceInfo = typeof(UseMiddlewareExtensions).GetMethod(nameof(GetService), BindingFlags.NonPublic | BindingFlags.Static);

        public static IRealmStoreBuilder<TState> UseMiddleware<TState, TMiddleware>(this IRealmStoreBuilder<TState> builder, params object[] args)
        {
            return builder.UseMiddleware(typeof(TMiddleware), args);
        }

        public static IRealmStoreBuilder<TState> UseMiddleware<TState>(this IRealmStoreBuilder<TState> builder, Type middleware, params object[] args)
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
                    throw new InvalidOperationException("ERROR 1");
                }

                MethodInfo methodinfo = invokeMethods[0];
                if (!typeof(TState).IsAssignableFrom(methodinfo.ReturnType))
                {
                    throw new InvalidOperationException("ERROR 2");
                }

                ParameterInfo[] parameters = methodinfo.GetParameters();
                if (parameters.Length != 1 || parameters[0].ParameterType != typeof(IAction))
                {
                    throw new InvalidOperationException("ERROR 3");
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

        private static object GetService(IServiceProvider sp, Type type, Type middleware)
        {
            var service = sp.GetService(type);
            if (service == null)
            {
                throw new InvalidOperationException();
            }

            return service;
        }
    }
}
