﻿using MVVMSample.Contracts;
using MVVMSample.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Mvvm;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;

namespace MVVMSample.ViewModels
{
    public class MainViewModel : ViewModel
    {
        #region Commands

        public ICommand ShowAddCarCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    var lstWindow = new AddCarDialog();
                    lstWindow.Owner = Application.Current.MainWindow;
                    lstWindow.ShowDialog();
                });
            }
        }

        public ICommand ShowListWindowCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    var lstWindow = new ListWindow();
                    lstWindow.Owner = Application.Current.MainWindow;
                    lstWindow.ShowDialog();
                });
            }
        }

        public ICommand ShowSearchListWindowCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    var lstWindow = new SearchListWindow();
                    lstWindow.Owner = Application.Current.MainWindow;
                    lstWindow.ShowDialog();
                });
            }
        }

        public ICommand ShowSearchTreeListWindowCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    var lstWindow = new TreeSearchListWindow();
                    lstWindow.Owner = Application.Current.MainWindow;
                    lstWindow.ShowDialog();
                });
            }
        }

        public ICommand ShowAlertCommand
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                     await UI.ShowAlertAsync("YAY!", "You confirmed that");
                });
            }
        }

        public ICommand ShowConfirmCommand
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    var result = await UI.ShowConfirmationDialogAsync("Confirm this", "This is a platform specific confirmation message");

                    if (result)
                        await UI.ShowAlertAsync("YAY!", "You confirmed that");
                });
            }
        }

        public ICommand ShowCustomUiCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    
                    var customUI = ServiceHost.GetRequiredService<ITestCustomUIProvider>();

                    customUI.SayHello();

                });
            }
        }

        public ICommand TestInvokeUIThreadOffUIThread
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    await Task.Run(async () =>
                    {
                        await UI.InvokeOnUIThreadAsync(() =>
                        {
                            ((MainWindow)Application.Current.MainWindow).panel.Visibility = Visibility.Visible;
                        });
                        
                    });
                   

                });
            }
        }

        public ICommand TestInvokeUIThreadOnUIThread
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    await UI.InvokeOnUIThreadAsync(() =>
                    {
                        ((MainWindow)Application.Current.MainWindow).panel.Visibility = Visibility.Visible;
                    });

                });
            }
        }

        public ICommand GetUIProviderService
        {
            get
            {
                return new DelegateCommand(() =>
                {
                  var wpfProvider = ServiceHost.GetRequiredService<IWPFPlatformUIProvider>();

                   var mainWindow = wpfProvider.CurrentApplication.MainWindow;
                });
            }
        }

        public ICommand ShowCommandBindingWindow
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    var lstWindow = new ThreadingTestWindow();
                    lstWindow.Owner = Application.Current.MainWindow;
                    lstWindow.ShowDialog();
                });
            }
        }

        #endregion
        public MainViewModel() : base()
        {

        }

        private void BackgroundTask(object thing)
        {
            ((MainWindow)Application.Current.MainWindow).panel.Visibility = Visibility.Visible;

            //MessageBox.Show("This is on the UI Thread, hopefully");
        }


    }
}
