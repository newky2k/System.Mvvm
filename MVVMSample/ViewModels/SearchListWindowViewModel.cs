using MVVMSample.Models;
using MVVMSample.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Mvvm;
using System.Text;
using System.Threading.Tasks;

namespace MVVMSample.ViewModels
{
    public class SearchListWindowViewModel : SearchViewModel<CarModel, List<CarModel>>
    {
        public override async Task RefreshAsync()
        {
            Items = CarRepository.Instance.Cars;
        }

        protected override List<CarModel> ApplyFilter(List<CarModel> data)
        {
            throw new NotImplementedException();
        }
    }
}
