using System;
using System.Collections.Generic;
using System.Linq;
using System.Mvvm.Contracts;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace DSoft.System.Mvvm.Uwp
{
    internal class PlatformUIProvider : IPlatformCoreUIProvider
    {
        public async Task ShowAlertAsync(string title, string message)
        {
            ContentDialog noWifiDialog = new ContentDialog
            {
                Title = title,
                Content = message,
                CloseButtonText = "Ok"
            };

            await noWifiDialog.ShowAsync();
        }

        public async Task<bool> ShowConfirmationDialogAsync(string title, string message)
        {
            var locationPromptDialog = new ContentDialog
            {
                Title = title,
                Content = message,
                CloseButtonText = "No",
                PrimaryButtonText = "Yes"
            };

            var result = await locationPromptDialog.ShowAsync();

            return (result == ContentDialogResult.Primary);
        }
    }
}
