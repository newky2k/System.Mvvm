using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace System.Mvvm
{
	/// <summary>
	/// Interface for core UI functionality
	/// </summary>
	public interface IPlatformCoreUIProvider
    {
		/// <summary>
		/// Invokes the on UI thread.
		/// </summary>
		/// <param name="action">The action.</param>
		void InvokeOnUIThread(Action action);

		/// <summary>
		/// Invokes the on UI thread asynchronous.
		/// </summary>
		/// <param name="action">The action.</param>
		/// <returns></returns>
		Task InvokeOnUIThreadAsync(Action action);

		/// <summary>
		/// Shows an alert
		/// </summary>
		/// <param name="title">The title.</param>
		/// <param name="message">The message.</param>
		/// <returns></returns>
		Task ShowAlertAsync(string title, string message);

		/// <summary>
		/// Show a confirmation dialog
		/// </summary>
		/// <param name="title">The title.</param>
		/// <param name="message">The message.</param>
		/// <returns></returns>
		Task<bool> ShowConfirmationDialogAsync(string title, string message);
    }
}
