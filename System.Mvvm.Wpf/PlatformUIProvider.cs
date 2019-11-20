using System;
using System.Collections.Generic;
using System.Linq;
using System.Mvvm.Contracts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace System.Mvvm.Wpf
{
    internal class PlatformUIProvider : IPlatformCoreUIProvider
    {
        public void ShowAlert(string title, string message)
        {
            MessageBox.Show(message, title);
        }

        public bool ShowConfirmationDialog(string title, string message)
        {
            var confirm = MessageBox.Show(message, title, MessageBoxButton.YesNo);

            return (confirm == MessageBoxResult.Yes);

        }
    }
}
