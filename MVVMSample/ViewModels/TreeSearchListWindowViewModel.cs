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
    public class TreeSearchListWindowViewModel : SearchTreeViewModel<CarModel, List<CarModel>>
    {
        public override async Task RefreshAsync()
        {
            Items = CarRepository.Instance.Cars;
        }

        protected override List<CarModel> ApplyFilter(List<CarModel> data)
        {
            throw new NotImplementedException();
        }

        protected override IReadOnlyCollection<List<CarModel>> BuildTreePath(List<CarModel> data)
        {
            throw new NotImplementedException();
        }
    }
}
