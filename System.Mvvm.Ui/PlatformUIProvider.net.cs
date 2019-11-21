using System;
using System.Collections.Generic;
using System.Linq;
using System.Mvvm.Contracts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace System.Mvvm.Ui
{
    internal class PlatformUIProvider : IPlatformCoreUIProvider
    {
        public async Task ShowAlertAsync(string title, string message)
        {
            await Task.Run(() =>
            {
                MessageBox.Show(message, title);
            });
            
        }


        public async Task<bool> ShowConfirmationDialogAsync(string title, string message)
        {
            //var tsk = new TaskCompletionSource<bool>();
            var result = false;

            await Task.Run(() =>
            {
                var confirm = MessageBox.Show(message, title, MessageBoxButton.YesNo);

                result = (confirm == MessageBoxResult.Yes);
            });

            return result;   
        }
    }
}
