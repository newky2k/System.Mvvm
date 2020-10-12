using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace System.Mvvm.Contracts
{
    public interface IPlatformCoreUIProvider
    {
        void InvokeOnUIThread(Action action);

        Task InvokeOnUIThreadAsync(Action action);

        Task ShowAlertAsync(string title, string message);

        Task<bool> ShowConfirmationDialogAsync(string title, string message);
    }
}
