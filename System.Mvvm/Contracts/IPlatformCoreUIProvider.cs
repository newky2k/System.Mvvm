using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace System.Mvvm.Contracts
{
    public interface IPlatformCoreUIProvider
    {
        Task InvokeOnUIThread(Action action);

        Task ShowAlertAsync(string title, string message);

        Task<bool> ShowConfirmationDialogAsync(string title, string message);
    }
}
