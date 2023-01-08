using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace System.Mvvm
{
	/// <summary>
	/// MvvmManager management class for initializing System.Mvvm.UI on Xamarin.Forms
	/// </summary>
	public partial class MvvmManager
    {
		/// <summary>
		/// Initializes the MvvmManager, with the assmebly calling the Init method
		/// </summary>
		public static void Init()
        {
            UI.PlatformProvider = PlatformUIProvider.Instance;

            //Centralised management of errors notifications through the ViewModel
            ViewModel.OnErrorOccurred += async (object sen, Exception e2) =>
            {
                if (e2 is TitledException)
                {
                    var ex = e2 as TitledException;

                    await UI.ShowAlertAsync(ex.Title, e2.Message);
                }
                else
                {

                    await UI.ShowAlertAsync("System Error", e2.Message);
                }

            };

            DelegateCommand.RequeryCommandsOnChange = false;

            DelegateCommand.RunOnUiThreadAction = (act) => UI.InvokeOnUIThread(act);

        }



    }
}
