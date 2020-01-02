using System;
using System.Collections.Generic;
using System.Linq;
using System.Mvvm.Contracts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace System.Mvvm.Ui
{
    internal class PlatformUIProvider : IPlatformCoreUIProvider
    {
        public Application CurrentApplication => Application.Current;

        public Dispatcher CurrentDispatcher => CurrentApplication.Dispatcher;

        public Window CurrentWindow => CurrentApplication.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive);

        public async Task InvokeOnUIThread(Action action)
        {
            if (CurrentDispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                await CurrentDispatcher.InvokeAsync(action);
            }
                
        }
        public async Task ShowAlertAsync(string title, string message)
        {
            await InvokeOnUIThread(() =>
            {
                MessageBox.Show(message, title);
            });

        }


        public async Task<bool> ShowConfirmationDialogAsync(string title, string message)
        {
            //var tsk = new TaskCompletionSource<bool>();
            var result = false;

            await InvokeOnUIThread(() =>
            {
                var confirm = MessageBox.Show(message, title, MessageBoxButton.YesNo);

                result = (confirm == MessageBoxResult.Yes);
            });

            return result;   
        }
    }
}
