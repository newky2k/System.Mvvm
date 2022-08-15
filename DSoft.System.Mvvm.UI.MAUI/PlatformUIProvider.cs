using System.Mvvm;

namespace System.Mvvm
{
	// All the code in this file is included in all platforms.
	internal partial class PlatformUIProvider : IMAUIPlatformUIProvider
	{
		private static readonly Lazy<PlatformUIProvider> _instance = new Lazy<PlatformUIProvider>(() => new PlatformUIProvider());
		internal static PlatformUIProvider Instance => _instance.Value;

		public Application CurrentApplication => Application.Current;

		public Task ShowAlertAsync(string title, string message) => CurrentApplication.MainPage.DisplayAlert(title, message, "OK");

		public Task<bool> ShowConfirmationDialogAsync(string title, string message) => CurrentApplication.MainPage.DisplayAlert(title, message, "Ok", "Cancel");

		public void InvokeOnUIThread(Action action) => MainThread.BeginInvokeOnMainThread(action);

		public Task InvokeOnUIThreadAsync(Action action) => MainThread.InvokeOnMainThreadAsync(action);

	}

	public interface IMAUIPlatformUIProvider : IPlatformCoreUIProvider
	{
		Application CurrentApplication { get; }
	}
}