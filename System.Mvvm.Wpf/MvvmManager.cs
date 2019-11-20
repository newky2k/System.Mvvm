using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace System.Mvvm.Wpf
{
    public class MvvmManager
    {
        private static Action<string, string> _messageDisplayHandler;

        private static void ShowAlert(string title, string message)
        {
            if (_messageDisplayHandler != null)
                _messageDisplayHandler(title, message);
            else
                MessageBox.Show(message, title);
        }

        /// <summary>
        /// Initializes the MvvmManager
        /// </summary>
        public static void Init()
        {
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

                    ShowAlert(ex.Title, e2.Message);
                }
                else
                {

                    ShowAlert("System Error", e2.Message);
                }

            };

            //Register the Main UI provider
            UI.Register<PlatformUIProvider>();
        }

        /// <summary>
        /// Initializes the MvvmManager and provides and override for displaying messages
        /// </summary>
        /// <param name="messageDisplayHandler">The message display handler.</param>
        public static void Init(Action<string, string> messageDisplayHandler)
        {
            Init();

            _messageDisplayHandler = messageDisplayHandler;
        }
    }
}
