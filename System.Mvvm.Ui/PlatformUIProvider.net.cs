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
    internal class PlatformUIProvider : IPlatformCoreUIProvider, WPFPlatformUIProvider
    {
        public Application CurrentApplication => Application.Current;

        public Dispatcher CurrentDispatcher => CurrentApplication.Dispatcher;

        public Window CurrentWindow => CurrentApplication.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive);

        /// <summary>
        /// Gets or sets the show alert overide function.
        /// </summary>
        /// <value>
        /// The show alert overide function.
        /// </value>
        public Func<string, string, Task> ShowAlertOverideFunction { get; set; }

        /// <summary>
        /// Gets or sets the show confirm overide function.
        /// </summary>
        /// <value>
        /// The show confirm overide function.
        /// </value>
        public Func<string, string, Task<bool>> ShowConfirmOverideFunction { get; set; }

        public Task InvokeOnUIThread(Action action)
        {
            if (CurrentDispatcher.CheckAccess())
            {
                action();

                return Task.CompletedTask;
            }
            else
            {
                return CurrentDispatcher.InvokeAsync(action).Task;
            }
                
        }

        public async Task ShowAlertAsync(string title, string message)
        {
            if (ShowAlertOverideFunction != null)
            {
                await InvokeOnUIThread(async () =>
                {
                    await ShowAlertOverideFunction(title, message);

                });
            }
            else
            {
                await InvokeOnUIThread(() =>
                {
                    MessageBox.Show(message, title);
                });
            }


        }

        public async Task<bool> ShowConfirmationDialogAsync(string title, string message)
        {
            //var tsk = new TaskCompletionSource<bool>();
            var result = false;

            if (ShowConfirmOverideFunction != null)
            {
                return await ShowConfirmOverideFunction(title, message);
            }
            else
            {
                await InvokeOnUIThread(() =>
                {
                    var confirm = MessageBox.Show(message, title, MessageBoxButton.YesNo);

                    result = (confirm == MessageBoxResult.Yes);
                });
            }

                

            return result;   
        }


    }

    public interface WPFPlatformUIProvider : IPlatformCoreUIProvider
    {
        Application CurrentApplication { get; }

        Dispatcher CurrentDispatcher { get; }

        Window CurrentWindow { get; }

        /// <summary>
        /// Gets or sets the show alert overide function, to provide a different implementation of the WPF show alert
        /// </summary>
        /// <value>
        /// The show alert overide function.
        /// </value>
        Func<string, string, Task> ShowAlertOverideFunction { get; set; }

        /// <summary>
        /// Gets or sets the show confirm overide function, to provide a different implementation of the WPF confirm dialog
        /// </summary>
        /// <value>
        /// The show confirm overide function.
        /// </value>
        Func<string, string, Task<bool>> ShowConfirmOverideFunction { get; set; }

    }
}
