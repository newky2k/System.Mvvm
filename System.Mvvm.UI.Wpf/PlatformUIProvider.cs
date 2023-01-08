using System;
using System.Collections.Generic;
using System.Linq;
using System.Mvvm;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace System.Mvvm
{
	/// <summary>
	/// WPF implementation of PlatformUIProvider.
	/// Implements the <see cref="IWPFPlatformUIProvider" />
	/// </summary>
	/// <seealso cref="IWPFPlatformUIProvider" />
	internal partial class PlatformUIProvider : IWPFPlatformUIProvider
    {
        private static readonly Lazy<PlatformUIProvider> _instance = new Lazy<PlatformUIProvider>(() => new PlatformUIProvider());

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <value>The instance.</value>
		internal static PlatformUIProvider Instance => _instance.Value;

		/// <summary>
		/// Gets the current application.
		/// </summary>
		/// <value>The current application.</value>
		public Application CurrentApplication => Application.Current;

		/// <summary>
		/// Gets the current dispatcher.
		/// </summary>
		/// <value>The current dispatcher.</value>
		public Dispatcher CurrentDispatcher => CurrentApplication.Dispatcher;

		/// <summary>
		/// Gets the current window.
		/// </summary>
		/// <value>The current window.</value>
		public Window CurrentWindow => CurrentApplication.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive);

		/// <summary>
		/// Gets or sets the show alert overide function.
		/// </summary>
		/// <value>The show alert overide function.</value>
		public Func<string, string, Task> ShowAlertOverideFunction { get; set; }

		/// <summary>
		/// Gets or sets the show confirm overide function.
		/// </summary>
		/// <value>The show confirm overide function.</value>
		public Func<string, string, Task<bool>> ShowConfirmOverideFunction { get; set; }

		/// <summary>
		/// Invokes the on UI thread.
		/// </summary>
		/// <param name="action">The action.</param>
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

		/// <summary>
		/// Invokes the on UI thread asynchronous.
		/// </summary>
		/// <param name="action">The action.</param>
		/// <returns>Task.</returns>
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

		/// <summary>
		/// Show alert as an asynchronous operation.
		/// </summary>
		/// <param name="title">The title.</param>
		/// <param name="message">The message.</param>
		/// <returns>A Task representing the asynchronous operation.</returns>
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

		/// <summary>
		/// Show confirmation dialog as an asynchronous operation.
		/// </summary>
		/// <param name="title">The title.</param>
		/// <param name="message">The message.</param>
		/// <returns>A Task&lt;System.Boolean&gt; representing the asynchronous operation.</returns>
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

		/// <summary>
		/// Gets the first window of the specified type
		/// </summary>
		/// <typeparam name="T">The type of window to locate</typeparam>
		/// <returns>T.</returns>
		public T CurrentWindowOfType<T>()  where T : Window => CurrentApplication.Windows.OfType<T>().FirstOrDefault(x => x.IsActive);

    }

	/// <summary>
	/// WPF specific interface for IPlatformCoreUIProvider
	/// Extends the <see cref="IPlatformCoreUIProvider" />
	/// </summary>
	/// <seealso cref="IPlatformCoreUIProvider" />
	public interface IWPFPlatformUIProvider : IPlatformCoreUIProvider
    {
		/// <summary>
		/// Gets the current application.
		/// </summary>
		/// <value>The current application.</value>
		Application CurrentApplication { get; }

		/// <summary>
		/// Gets the current dispatcher.
		/// </summary>
		/// <value>The current dispatcher.</value>
		Dispatcher CurrentDispatcher { get; }

		/// <summary>
		/// Gets the current window.
		/// </summary>
		/// <value>The current window.</value>
		Window CurrentWindow { get; }

		/// <summary>
		/// Gets or sets the show alert overide function, to provide a different implementation of the WPF show alert
		/// </summary>
		/// <value>The show alert overide function.</value>
		Func<string, string, Task> ShowAlertOverideFunction { get; set; }

		/// <summary>
		/// Gets or sets the show confirm overide function, to provide a different implementation of the WPF confirm dialog
		/// </summary>
		/// <value>The show confirm overide function.</value>
		Func<string, string, Task<bool>> ShowConfirmOverideFunction { get; set; }

		/// <summary>
		/// Gets the first window of the specified type
		/// </summary>
		/// <typeparam name="T">The type of window to locate</typeparam>
		/// <returns>T.</returns>
		T CurrentWindowOfType<T>() where T : Window;
    }
}
