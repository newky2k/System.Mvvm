#if UAP
using Windows.UI.Xaml.Controls;
#else
using Microsoft.UI.Xaml.Controls;
#endif
//
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
using Windows.UI.Popups;

namespace System.Mvvm
{

    internal partial class PlatformUIProvider : IUWPPlatformUIProvider
    {
        private static readonly Lazy<PlatformUIProvider> _instance = new Lazy<PlatformUIProvider>(() => new PlatformUIProvider());
        internal static PlatformUIProvider Instance => _instance.Value;

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

        public void InvokeOnUIThread(Action action)
        {
            var dispatcher = CoreApplication.MainView?.CoreWindow?.Dispatcher;

            if (dispatcher == null)
                throw new InvalidOperationException("Unable to find main thread.");

            dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action()).WatchForError();
        }

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

    public interface IUWPPlatformUIProvider : IPlatformCoreUIProvider
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
