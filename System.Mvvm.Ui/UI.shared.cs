﻿using System;
using System.Collections.Generic;
using System.Mvvm;
using System.Mvvm.Ui;
using System.Text;
using System.Threading.Tasks;

namespace System.Mvvm
{
    public static class UI
    {
        #region Core Backend

        private static Lazy<IPlatformCoreUIProvider> _instance = new Lazy<IPlatformCoreUIProvider>(() => new PlatformUIProvider());

        internal static IPlatformCoreUIProvider PlatformProvider => _instance.Value;

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

        public static IPlatformCoreUIProvider Provider() => _instance.Value;

        public static T Provider<T>() where T : IPlatformCoreUIProvider => (T)_instance.Value;
        #endregion

    }
}
