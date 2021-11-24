using System;
using System.Collections.Generic;
using System.Linq;
using System.Mvvm;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Xamarin.Forms;

namespace System.Mvvm
{
    internal partial class PlatformUIProvider : IXamarinFormsPlatformUIProvider
    {
        public Application CurrentApplication => Application.Current;

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
            //if (CurrentDispatcher.CheckAccess())
            //{
            //    action();
            //}
            //else
            //{
            //    CurrentDispatcher.Invoke(action);
            //}
        }

        public Task InvokeOnUIThreadAsync(Action action)
        {
            return Task.FromResult(true);

            //if (CurrentDispatcher.CheckAccess())
            //{
            //    action();

            //    return Task.CompletedTask;
            //}
            //else
            //{
            //    return CurrentDispatcher.InvokeAsync(action).Task;
            //}
                
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
                    //MessageBox.Show(message, title);
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
                    //var confirm = MessageBox.Show(message, title, MessageBoxButton.YesNo);

                    //result = (confirm == MessageBoxResult.Yes);
                });
            }

                

            return result;   
        }


    }

    public interface IXamarinFormsPlatformUIProvider : IPlatformCoreUIProvider
    {
        Application CurrentApplication { get; }
    }
}
