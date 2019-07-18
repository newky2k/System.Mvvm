using MVVMSample.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Mvvm;
using System.Text;
using System.Threading.Tasks;
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

        #endregion
        public MainViewModel() : base()
        {

        }
    }
}
