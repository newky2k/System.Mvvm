using System;
using System.Collections.Generic;
using System.Text;

namespace System.Mvvm.Contracts
{
    public interface IPlatformCoreUIProvider
    {
        void ShowAlert(string title, string message);
        bool ShowConfirmationDialog(string title, string message);
    }
}
