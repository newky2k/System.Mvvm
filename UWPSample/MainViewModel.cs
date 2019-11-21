using System;
using System.Collections.Generic;
using System.Linq;
using System.Mvvm;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;

namespace UWPSample
{
    public class MainViewModel : ViewModel
    {
        private string _username;

        public string Username
        {
            get { return _username; }
            set 
            {
                _username = value; 
                NotifyPropertyChanged(nameof(Username)); 
            }
        }


        public ICommand DoSomthingCommand
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                   var result = await UI.ShowConfirmationDialogAsync("Boom!", "Showing you this message");

                    if (result)
                    {
                        await UI.ShowAlertAsync("Confirmed", "Init!");
                    }
                }, 
                ()=>
                {
                    return !string.IsNullOrWhiteSpace(Username);
                });
            }
        }
    }
}
