﻿using MVVMSample.Models;
using MVVMSample.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Mvvm;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MVVMSample.ViewModels
{
    public class TreeSearchListWindowViewModel : SearchTreeViewModel<CarModel, ObservableCollection<CarModel>>
    {
        public override async Task RefreshAsync()
        {
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

        protected override IReadOnlyCollection<IEnumerable<CarModel>> BuildTreePath(IEnumerable<CarModel> data)
        {
            throw new NotImplementedException();
        }
    }
}
