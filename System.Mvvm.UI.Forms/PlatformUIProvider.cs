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
	/// <summary>
	/// Xamarin.Form immplementation of PlatformUIProvider
	/// Implements the <see cref="IXamarinFormsPlatformUIProvider" />
	/// </summary>
	/// <seealso cref="IXamarinFormsPlatformUIProvider" />
	internal partial class PlatformUIProvider : IXamarinFormsPlatformUIProvider
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
		public void InvokeOnUIThread(Action action) => Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(action);

		/// <summary>
		/// Invokes the on UI thread asynchronous.
		/// </summary>
		/// <param name="action">The action.</param>
		/// <returns>Task.</returns>
		public Task InvokeOnUIThreadAsync(Action action) => Xamarin.Essentials.MainThread.InvokeOnMainThreadAsync(action);

    }

	/// <summary>
	/// Xamarin.Forms interface for  IPlatformCoreUIProvider
	/// Extends the <see cref="IPlatformCoreUIProvider" />
	/// </summary>
	/// <seealso cref="IPlatformCoreUIProvider" />
	public interface IXamarinFormsPlatformUIProvider : IPlatformCoreUIProvider
    {
		/// <summary>
		/// Gets the current application.
		/// </summary>
		/// <value>The current application.</value>
		Application CurrentApplication { get; }
    }
}
