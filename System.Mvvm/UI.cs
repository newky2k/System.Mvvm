using System;
using System.Collections.Generic;
using System.Linq;
using System.Mvvm.Attributes;
using System.Mvvm.Contracts;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace System.Mvvm
{
    public static class UI
    {
        #region Core Backend
        private static IPlatformCoreUIProvider _mainUiProvider;

        internal static IPlatformCoreUIProvider PlatformProvider
        {
            get 
            {
                if (_mainUiProvider == null)
                    throw new Exception("The platform specific UI provider has not been set.  Call System.Mvvm.UI.Init to register the platform implementation");

                return _mainUiProvider; 
            }
            private set { _mainUiProvider = value; }
        }
        #endregion

        #region Initializers
        /// <summary>
        /// Initializes the core UI provider
        /// </summary>
        /// <typeparam name="T">Implementation of IPlatformCoreUIProvider</typeparam>
        public static void Init<T>() where T : IPlatformCoreUIProvider, new()
        {
            PlatformProvider = new T();

            var assms = new List<Assembly>() { Assembly.GetCallingAssembly() };

            LoadServices(assms);
        }


        /// <summary>
        /// Initializes the core UI provider
        /// </summary>
        /// <typeparam name="T">Implementation of IPlatformCoreUIProvider</typeparam>
        /// <param name="assemblies">External Assemblies with UI services</param>
        public static void Init<T>(params Assembly[] assemblies) where T : IPlatformCoreUIProvider, new()
        {
            PlatformProvider = new T();

            var assms = new List<Assembly>() { Assembly.GetCallingAssembly() };

            if (assemblies != null && assemblies.Count() > 0)
            {
                foreach (var aAssm in assemblies)
                {
                    if (!assms.Contains(aAssm))
                        assms.Add(aAssm);
                }
            }
            
            LoadServices(assms);
        }

        /// <summary>
        /// Initializes the core UI provider
        /// </summary>
        /// <typeparam name="T">Implementation of IPlatformCoreUIProvider</typeparam>
        /// <param name="types">Types in external assemblies with UI services</param>
        public static void Init<T>(params Type[] types) where T : IPlatformCoreUIProvider, new()
        {
            PlatformProvider = new T();

            var assms = new List<Assembly>() { Assembly.GetCallingAssembly() };

            if (types != null && types.Count() > 0)
            {
                foreach (var aAssm in types.Select(x => x.Assembly))
                {
                    if (!assms.Contains(aAssm))
                        assms.Add(aAssm);
                }
            }

            LoadServices(assms);

        }

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

        #region UI Providers

        /// <summary>
        /// Stored instances of the 
        /// </summary>
        private static Dictionary<Type, object> Services { get; set; } = new Dictionary<Type, object>();

        /// <summary>
        /// Register types
        /// </summary>
        private static List<Type> ServiceTypes { get; set; } = new List<Type>();


        /// <summary>
        /// Register a UI Service
        /// </summary>
        /// <typeparam name="T">Service implementation type</typeparam>
        public static void Register<T>() where T : new()
        {
            var newType = typeof(T);

            if (!ServiceTypes.Contains(newType))
                ServiceTypes.Add(newType);
        }

        /// <summary>
        /// Register all UI Services in the specified assemblies
        /// </summary>
        /// <param name="assemblies">Assemblies to process</param>
        public static void Register(params Assembly[] assemblies)
        {
            var assms = new List<Assembly>() { Assembly.GetCallingAssembly() };

            if (assemblies != null && assemblies.Count() > 0)
            {
                foreach (var aAssm in assemblies)
                {
                    if (!assms.Contains(aAssm))
                        assms.Add(aAssm);
                }
            }

            LoadServices(assms);
        }

        /// <summary>
        /// Register all UI Services in the assemblies conatining the specified types
        /// </summary>
        /// <param name="types">Types to process in external assemblies</param>
        public static void Register(params Type[] types)
        {
            var assms = new List<Assembly>() { Assembly.GetCallingAssembly() };

            if (types != null && types.Count() > 0)
            {
                foreach (var aAssm in types.Select(x => x.Assembly))
                {
                    if (!assms.Contains(aAssm))
                        assms.Add(aAssm);
                }
            }

            LoadServices(assms);

        }

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

            var type = ServiceTypes.FirstOrDefault(x => x.Equals(typeof(T)) || typeof(T).IsAssignableFrom(x));

            if (type != null)
            {
                if (!cachedInstance == true)
                {
                    return (T)Activator.CreateInstance(type);
                }

                if (Services.ContainsKey(type))
                    return (T)Services[type];
                else
                {
                    var newType = (T)Activator.CreateInstance(type);

                    Services.Add(type, newType);

                    return newType;
                }
            }

            throw new Exception("Type not registered");
        }

        private static void LoadServices(IEnumerable<Assembly> assemblies)
        {
            var custAttr = typeof(UIServiceAttribute);

            foreach (var assembly in assemblies)
            {
                var serAttrs = assembly.GetCustomAttributes(custAttr, true);

                foreach (UIServiceAttribute attrib in serAttrs)
                {
                    if (!ServiceTypes.Contains(attrib.Implementation))
                        ServiceTypes.Add(attrib.Implementation);
                }
            }
        }
        #endregion


    }

}
