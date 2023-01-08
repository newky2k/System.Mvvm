using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Mvvm;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI.Core;

namespace System.Mvvm
{


	/// <summary>
	/// WinUI implementaion of PlatformUIProvider.
	/// Implements the <see cref="IDesktopPlatformUIProvider" />
	/// </summary>
	/// <seealso cref="IDesktopPlatformUIProvider" />
	internal partial class PlatformUIProvider : IDesktopPlatformUIProvider
	{
        private static readonly Lazy<PlatformUIProvider> _instance = new Lazy<PlatformUIProvider>(() => new PlatformUIProvider());

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <value>The instance.</value>
		internal static PlatformUIProvider Instance => _instance.Value;

		/// <summary>
		/// Gets a value indicating whether this instance is main thread.
		/// </summary>
		/// <value><c>true</c> if this instance is main thread; otherwise, <c>false</c>.</value>
		static bool IsMainThread
        {
            get
            {
                // if there is no main window, then this is either a service
                // or the UI is not yet constructed, so the main thread is the
                // current thread
                try
                {
                    if (CoreApplication.MainView?.CoreWindow == null)
                        return true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Unable to validate MainView creation. {ex.Message}");
                    return true;
                }

                return CoreApplication.MainView.CoreWindow.Dispatcher?.HasThreadAccess ?? false;
            }
        }

		/// <summary>
		/// Invokes the on UI thread asynchronous.
		/// </summary>
		/// <param name="action">The action.</param>
		/// <returns>Task.</returns>
		public Task InvokeOnUIThreadAsync(Action action)
        {
            if (IsMainThread)
            {
                action();

                return Task.CompletedTask;

            }

            var tcs = new TaskCompletionSource<bool>();

            InvokeOnUIThread(() =>
            {
                try
                {
                    action();
                    tcs.TrySetResult(true);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            });

            return tcs.Task;


        }

		/// <summary>
		/// Invokes the on UI thread.
		/// </summary>
		/// <param name="action">The action.</param>
		/// <exception cref="System.InvalidOperationException">Unable to find main thread.</exception>
		public void InvokeOnUIThread(Action action)
        {
            var dispatcher = CoreApplication.MainView?.CoreWindow?.Dispatcher;

            if (dispatcher == null)
                throw new InvalidOperationException("Unable to find main thread.");

            dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action()).WatchForError();
        }

		/// <summary>
		/// Show alert as an asynchronous operation.
		/// </summary>
		/// <param name="title">The title.</param>
		/// <param name="message">The message.</param>
		/// <returns>A Task representing the asynchronous operation.</returns>
		public async Task ShowAlertAsync(string title, string message)
        {
            ContentDialog noWifiDialog = new ContentDialog
            {
                Title = title,
                Content = message,
                CloseButtonText = "Ok"
            };

            await noWifiDialog.ShowAsync();
        }

		/// <summary>
		/// Show confirmation dialog as an asynchronous operation.
		/// </summary>
		/// <param name="title">The title.</param>
		/// <param name="message">The message.</param>
		/// <returns>A Task&lt;System.Boolean&gt; representing the asynchronous operation.</returns>
		public async Task<bool> ShowConfirmationDialogAsync(string title, string message)
        {
            var locationPromptDialog = new ContentDialog
            {
                Title = title,
                Content = message,
                CloseButtonText = "No",
                PrimaryButtonText = "Yes"
            };

            var result = await locationPromptDialog.ShowAsync();

            return (result == ContentDialogResult.Primary);
        }

    }

	/// <summary>
	/// IDesktopPlatformUIProvider for WINUI and UWP
	/// Extends the <see cref="IDesktopPlatformUIProvider" />
	/// </summary>
	/// <seealso cref="IDesktopPlatformUIProvider" />
	public interface IDesktopPlatformUIProvider : IPlatformCoreUIProvider
	{

	}

	internal static partial class MainThreadExtensions
    {
        internal static void WatchForError(this IAsyncAction self) =>
            self.AsTask().WatchForError();

        internal static void WatchForError<T>(this IAsyncOperation<T> self) =>
            self.AsTask().WatchForError();

        internal static void WatchForError(this Task self)
        {
            var context = SynchronizationContext.Current;
            if (context == null)
                return;

            self.ContinueWith(
                t =>
                {
                    var exception = t.Exception.InnerExceptions.Count > 1 ? t.Exception : t.Exception.InnerException;

                    context.Post(e => { throw (Exception)e; }, exception);
                }, CancellationToken.None,
                TaskContinuationOptions.OnlyOnFaulted,
                TaskScheduler.Default);
        }
    }
}
