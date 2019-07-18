using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Mvvm;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MVVMSample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //call this to enable monitoring of the ExecuteChanged events
            DelegateCommand.ExecuteChanged = (ev, shoudlAdd) =>
            {
                if (shoudlAdd)
                {
                    CommandManager.RequerySuggested += ev;
                }
                else
                {
                    CommandManager.RequerySuggested -= ev;
                }
            };

            //Centralised management of errors notifications through the ViewModel
            ViewModel.OnErrorOccured += (object sen, Exception e2) =>
            {
                if (e2 is TitledException)
                {
                    var ex = e2 as TitledException;

                    MessageBox.Show(e2.Message,ex.Title);
                }
                else
                {

                    MessageBox.Show(e2.Message, "System Error");
                }

            };
        }
    }
}
