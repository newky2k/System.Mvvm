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
    public class SearchListWindowViewModel : SearchViewModel<CarModel, ObservableCollection<CarModel>>
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
            await Task.Delay(1);

            Items = CarRepository.Instance.Cars;
        }

        protected override void HandleItemSelection(CarModel selection)
        {
            MessageBox.Show($"Selected: {selection.Make} {selection.Model}");
        }

        protected override ObservableCollection<CarModel> ApplyFilter(ObservableCollection<CarModel> data)
        {
            if (string.IsNullOrWhiteSpace(SearchText))
                return data;

            return data.Where(x => x.Make.ToLower().Contains(SearchText.ToLower()) || x.Model.ToLower().Contains(SearchText.ToLower())).ToObservable();
        }
    }
}
