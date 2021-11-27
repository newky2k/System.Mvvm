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
        private static readonly Lazy<PlatformUIProvider> _instance = new Lazy<PlatformUIProvider>(() => new PlatformUIProvider());
        internal static PlatformUIProvider Instance => _instance.Value;

        public Application CurrentApplication => Application.Current;

        public Task ShowAlertAsync(string title, string message) => CurrentApplication.MainPage.DisplayAlert(title, message, "OK");

        public Task<bool> ShowConfirmationDialogAsync(string title, string message) => CurrentApplication.MainPage.DisplayAlert(title, message, "Ok", "Cancel");

        public void InvokeOnUIThread(Action action) => Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(action);

        public Task InvokeOnUIThreadAsync(Action action) => Xamarin.Essentials.MainThread.InvokeOnMainThreadAsync(action);

    }

    public interface IXamarinFormsPlatformUIProvider : IPlatformCoreUIProvider
    {
        Application CurrentApplication { get; }
    }
}
