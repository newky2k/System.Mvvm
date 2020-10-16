using System;
using System.Collections.Generic;
using System.Linq;
using System.Mvvm.Attributes;
using System.Mvvm.Contracts;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace System.Mvvm
{
    public static class UI
    {
        #region Core Backend
       

        internal static IPlatformCoreUIProvider PlatformProvider => Services.Get<IPlatformCoreUIProvider>();

        #endregion

        #region Core UI Methods

        /// <summary>
        /// Show an alert
        /// </summary>
        /// <param name="title">The title of the alert</param>
        /// <param name="message">The message to display</param>
        public static async Task ShowAlertAsync(string title, string message)
        {
            await PlatformProvider.ShowAlertAsync(title, message);
        }

        /// <summary>
        /// Show a confirmation dialog
        /// </summary>
        /// <param name="title">The title of the alert</param>
        /// <param name="message">The confirmaton message to display</param>
        /// <returns></returns>
        public static async Task<bool> ShowConfirmationDialogAsync(string title, string message)
        {
            return await PlatformProvider.ShowConfirmationDialogAsync(title, message);
        }

        /// <summary>
        /// Invokes the action on the UI thread
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        public static void InvokeOnUIThread(Action action) => PlatformProvider.InvokeOnUIThread(action);

        /// <summary>
        /// Invokes the action on the UI thread asyncronously
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        public static Task InvokeOnUIThreadAsync(Action action) => PlatformProvider.InvokeOnUIThreadAsync(action);


        #endregion

        #region Initializers

        [Obsolete("Use Services.Register insead")]
        /// <summary>
        /// Initializes the core UI provider
        /// </summary>
        /// <typeparam name="T">Implementation of IPlatformCoreUIProvider</typeparam>
        public static void Init<T>() where T : IPlatformCoreUIProvider, new() => Services.Register<T>();

        [Obsolete("Use Services.Register insead")]
        /// <summary>
        /// Initializes the core UI provider
        /// </summary>
        /// <typeparam name="T">Implementation of IPlatformCoreUIProvider</typeparam>
        /// <param name="assemblies">External Assemblies with UI services</param>
        public static void Init<T>(params Assembly[] assemblies) where T : IPlatformCoreUIProvider, new() => Services.Register<T>(assemblies);

        [Obsolete("Use Services.Register insead")]
        /// <summary>
        /// Initializes the core UI provider
        /// </summary>
        /// <typeparam name="T">Implementation of IPlatformCoreUIProvider</typeparam>
        /// <param name="types">Types in external assemblies with UI services</param>
        public static void Init<T>(params Type[] types) where T : IPlatformCoreUIProvider, new() => Services.Register<T>(types);

        #endregion

        #region UI Providers

        [Obsolete("Use Services.Register insead")]
        /// <summary>
        /// Register a UI Service
        /// </summary>
        /// <typeparam name="T">Service implementation type</typeparam>
        public static void Register<T>() where T : new() => Services.Register<T>();

        [Obsolete("Use Services.Register insead")]
        /// <summary>
        /// Register all UI Services in the specified assemblies
        /// </summary>
        /// <param name="assemblies">Assemblies to process</param>
        public static void Register(params Assembly[] assemblies) => Services.Register(assemblies);

        [Obsolete("Use Services.Register insead")]
        /// <summary>
        /// Register all UI Services in the assemblies conatining the specified types
        /// </summary>
        /// <param name="types">Types to process in external assemblies</param>
        public static void Register(params Type[] types) => Services.Register(types);

        [Obsolete("Use Services.Get insead")]
        /// <summary>
        /// Get a UI Service implementation
        /// </summary>
        /// <typeparam name="T">The inherited type</typeparam>
        /// <param name="cachedInstance">Cache the instance for use later, default is true</param>
        /// <returns></returns>
        public static T Get<T>(bool cachedInstance = true)
        {
            //check if the type is the Core platrofm provider interface
            if (typeof(T).Equals(typeof(IPlatformCoreUIProvider)) || typeof(IPlatformCoreUIProvider).IsAssignableFrom(typeof(T)))
                return (T)PlatformProvider;

            return Services.Get<T>(cachedInstance);

        }

        #endregion


    }

}
