using System;
using System.Collections.Generic;
using System.Linq;
using System.Mvvm;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System.Mvvm
{
    public class MvvmManager
    {
        /// <summary>
        /// Initializes the MvvmManager, with the assmebly calling the Init method
        /// </summary>
        public static void Init()
        {
            UI.PlatformProvider = new PlatformUIProvider();

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

            DelegateCommand.RequeryCommandsOnChange = true;
        }

    }
}
