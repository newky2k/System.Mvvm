using System.Mvvm;

namespace System.Mvvm
{
	// All the code in this file is included in all platforms.
	/// <summary>
	/// Class PlatformUIProvider.
	/// Implements the <see cref="IMAUIPlatformUIProvider" />
	/// </summary>
	/// <seealso cref="IMAUIPlatformUIProvider" />
	internal partial class PlatformUIProvider : IMAUIPlatformUIProvider
	{
		private static readonly Lazy<PlatformUIProvider> _instance = new Lazy<PlatformUIProvider>(() => new PlatformUIProvider());

		/// <summary>
		/// Gets the platform instance of PlatformUIProvider
		/// </summary>
		/// <value>The instance.</value>
		internal static PlatformUIProvider Instance => _instance.Value;

		/// <summary>
		/// Gets the current application.
		/// </summary>
		/// <value>The current application.</value>
		public Application CurrentApplication => Application.Current;

		/// <summary>
		/// Shows an alert
		/// </summary>
		/// <param name="title">The title.</param>
		/// <param name="message">The message.</param>
		/// <returns>Task.</returns>
		public Task ShowAlertAsync(string title, string message) => CurrentApplication.MainPage.DisplayAlert(title, message, "OK");

		/// <summary>
		/// Show a confirmation dialog
		/// </summary>
		/// <param name="title">The title.</param>
		/// <param name="message">The message.</param>
		/// <returns>Task&lt;System.Boolean&gt;.</returns>
		public Task<bool> ShowConfirmationDialogAsync(string title, string message) => CurrentApplication.MainPage.DisplayAlert(title, message, "Ok", "Cancel");

		/// <summary>
		/// Invokes the on UI thread.
		/// </summary>
		/// <param name="action">The action.</param>
		public void InvokeOnUIThread(Action action) => MainThread.BeginInvokeOnMainThread(action);

		/// <summary>
		/// Invokes the on UI thread asynchronously
		/// </summary>
		/// <param name="action">The action.</param>
		/// <returns>Task.</returns>
		public Task InvokeOnUIThreadAsync(Action action) => MainThread.InvokeOnMainThreadAsync(action);

	}

	/// <summary>
	/// MAUI specific interface for IPlatformCoreUIProvider
	/// Extends the <see cref="IPlatformCoreUIProvider" />
	/// </summary>
	/// <seealso cref="IPlatformCoreUIProvider" />
	public interface IMAUIPlatformUIProvider : IPlatformCoreUIProvider
	{
		/// <summary>
		/// Gets the current application.
		/// </summary>
		/// <value>The current application.</value>
		Application CurrentApplication { get; }
	}
}