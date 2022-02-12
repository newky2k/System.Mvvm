using System;
using System.Collections.Generic;
using System.Mvvm;
using System.Text;
using System.Threading.Tasks;

namespace System.Mvvm
{
	/// <summary>
	/// Core UI helper class for calling the Platform 
	/// </summary>
	public static class UI
    {
        #region Core Backend

        private static IPlatformCoreUIProvider _instance;

        internal static IPlatformCoreUIProvider PlatformProvider
        {
            get => _instance;
            set => _instance = value;
        }

        #endregion

        #region Core UI Methods

        /// <summary>
        /// Shows an alert
        /// </summary>
        /// <param name="title">The title of the alert</param>
        /// <param name="message">The message to display</param>
        public static Task ShowAlertAsync(string title, string message) => PlatformProvider.ShowAlertAsync(title, message);
        

        /// <summary>
        /// Shows a confirmation dialog
        /// </summary>
        /// <param name="title">The title of the alert</param>
        /// <param name="message">The confirmaton message to display</param>
        /// <returns></returns>
        public static Task<bool> ShowConfirmationDialogAsync(string title, string message) => PlatformProvider.ShowConfirmationDialogAsync(title, message);
        

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

        public static IPlatformCoreUIProvider Provider() => _instance;

        public static T Provider<T>() where T : IPlatformCoreUIProvider => (T)_instance;
        #endregion

    }
}
