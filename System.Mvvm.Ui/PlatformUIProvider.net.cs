using System;
using System.Collections.Generic;
using System.Linq;
using System.Mvvm.Attributes;
using System.Mvvm.Contracts;
using System.Mvvm.Ui;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

[assembly: MvvmService(typeof(PlatformUIProvider))]
namespace System.Mvvm.Ui
{
    [SingletonService]
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

        public void InvokeOnUIThread(Action action)
        {
            if (CurrentDispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                CurrentDispatcher.Invoke(action);
            }
        }

        public Task InvokeOnUIThreadAsync(Action action)
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
                await InvokeOnUIThreadAsync(async () =>
                {
                    await ShowAlertOverideFunction(title, message);

                });
            }
            else
            {
                await InvokeOnUIThreadAsync(() =>
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
                await InvokeOnUIThreadAsync(() =>
                {
                    var confirm = MessageBox.Show(message, title, MessageBoxButton.YesNo);

                    result = (confirm == MessageBoxResult.Yes);
                });
            }

                

            return result;   
        }

        public T CurrentWindowOfType<T>()  where T : Window => CurrentApplication.Windows.OfType<T>().FirstOrDefault(x => x.IsActive);

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

        /// <summary>
        /// Gets the first window of the specified type
        /// </summary>
        /// <typeparam name="T">The type of window to locate</typeparam>
        /// <returns></returns>
        T CurrentWindowOfType<T>() where T : Window;
    }
}
