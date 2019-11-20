﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Mvvm.Attributes;
using System.Mvvm.Contracts;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

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

        /// <summary>
        /// Initializes the core UI provider
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void Init<T>(IEnumerable<Assembly> assemblies = null) where T : IPlatformCoreUIProvider, new()
        {
            PlatformProvider = new T();

            if (assemblies != null && assemblies.Count() > 0)
                LoadServices(assemblies);
        }

        #endregion

        #region Core UI Methods

        /// <summary>
        /// Show an alert
        /// </summary>
        /// <param name="title">The title of the alert</param>
        /// <param name="message">The message to display</param>
        public static void ShowAlert(string title, string message)
        {
            PlatformProvider.ShowAlert(title, message);
        }

        /// <summary>
        /// Show a confirmation dialog
        /// </summary>
        /// <param name="title">The title of the alert</param>
        /// <param name="message">The confirmaton message to display</param>
        /// <returns></returns>
        public static bool ShowConfirmationDialog(string title, string message)
        {
            return PlatformProvider.ShowConfirmationDialog(title, message);
        }

        #endregion

        #region UI Providers

        /// <summary>
        /// The registered UI services
        /// </summary>
        public static Dictionary<Type, object> Services { get; set; } = new Dictionary<Type, object>();

        private static List<Type> ServiceTypes { get; set; } = new List<Type>();


        public static void Register<T>() where T : new()
        {
            var newType = typeof(T);

            if (!ServiceTypes.Contains(newType))
                ServiceTypes.Add(newType);
        }

        public static T Get<T>()
        {
            var type = ServiceTypes.FirstOrDefault(x => x.Equals(typeof(T)) || typeof(T).IsAssignableFrom(x));

            if (type != null)
            {
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
            var custAttr = typeof(MvvmServiceAttribute);

            foreach (var assembly in assemblies)
            {
                var serAttrs = assembly.GetCustomAttributes(custAttr, true);

                foreach (MvvmServiceAttribute attrib in serAttrs)
                {
                    if (!ServiceTypes.Contains(attrib.Implementation))
                        ServiceTypes.Add(attrib.Implementation);
                }
            }
        }
        #endregion


    }

}