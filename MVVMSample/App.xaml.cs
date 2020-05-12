using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MVVMSample.Providers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Mvvm;
using System.Mvvm.Ui;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MVVMSample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MvvmManager.Init();

            var wpfPlatform = UI.Get<WPFPlatformUIProvider>();

            wpfPlatform.ShowAlertOverideFunction = (title, message) => ShowAlertWindow(title, message);

            wpfPlatform.ShowConfirmOverideFunction = (title, message) => ShowConfirmationDialog(title, message);
        }

        public static Task ShowAlertWindow(string title, string message)
        {
            var wpfPlatform = UI.Get<WPFPlatformUIProvider>();

            var currentWindow = wpfPlatform.CurrentWindow as MetroWindow;

            return DialogManager.ShowMessageAsync(currentWindow, title, message);
        }

        public static Task<bool> ShowConfirmationDialog(string title, string message)
        {
            var tcs = new TaskCompletionSource<bool>();

            var wpfPlatform = UI.Get<WPFPlatformUIProvider>();

            UI.InvokeOnUIThread(async () =>
            {
                var currentWindow = wpfPlatform.CurrentWindow as MetroWindow;

                var result = await DialogManager.ShowMessageAsync(currentWindow, title, message, MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings()
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No",
                });

                tcs.SetResult((result == MessageDialogResult.Affirmative));
            });
            

            return tcs.Task;
        }
    }
}
