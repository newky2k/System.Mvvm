using System;
using System.Collections.Generic;
using System.Mvvm.Contracts;
using System.Text;
using System.Threading.Tasks;

namespace System.Mvvm.Ui
{
    internal partial class PlatformUIProvider : IPlatformCoreUIProvider
    {
        public void InvokeOnUIThread(Action action) => throw new PlatformNotSupportedException();

        public Task InvokeOnUIThreadAsync(Action action) => throw new PlatformNotSupportedException();

        public Task ShowAlertAsync(string title, string message) => throw new PlatformNotSupportedException();

        public Task<bool> ShowConfirmationDialogAsync(string title, string message) => throw new PlatformNotSupportedException();
    }
}
