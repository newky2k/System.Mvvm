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

    }

}
