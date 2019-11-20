using System;
using System.Collections.Generic;
using System.Mvvm.Contracts;
using System.Text;

namespace System.Mvvm
{
    public static class UI
    {
        private static IPlatformUIProvider _mainUiProvider;

        internal static IPlatformUIProvider PlatformProvider
        {
            get 
            {
                if (_mainUiProvider == null)
                    throw new Exception("The platform specific UI provider has not been set.  Call System.Mvvm.UI.Register to register the platform implementation");

                return _mainUiProvider; 
            }
            private set { _mainUiProvider = value; }
        }

        public static void Register<T>() where T : IPlatformUIProvider, new()
        {
            PlatformProvider = new T();
        }

        public static void ShowAlert(string title, string message)
        {
            PlatformProvider.ShowAlert(title, message);
        }

        public static bool ShowConfirmationDialog(string title, string message)
        {
           return PlatformProvider.ShowConfirmationDialog(title, message);
        }
    }

}
