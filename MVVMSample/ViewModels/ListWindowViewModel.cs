using MVVMSample.Models;
using MVVMSample.Repository;
using MVVMSample.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Mvvm;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MVVMSample.ViewModels
{
    public class ListWindowViewModel : ListViewModel<CarModel, ObservableCollection<CarModel>>
    {
        public ICommand AddCarCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    var addDlg = new AddCarDialog();
                    addDlg.Owner = Application.Current.MainWindow;
                    addDlg.ShowDialog();
                });
            }
        }
        public override async Task RefreshAsync()
        {
            Items = CarRepository.Instance.Cars;
        }

        protected override void HandleItemSelection(CarModel selection)
        {
            MessageBox.Show($"Selected: {selection.Make} {selection.Model}");
        }
    }
}
