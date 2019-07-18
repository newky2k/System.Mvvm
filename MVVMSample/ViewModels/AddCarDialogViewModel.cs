using MVVMSample.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Mvvm;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVMSample.ViewModels
{
    public class AddCarDialogViewModel : ViewModel
    {
        private string _make;
        private string _model;

        public string Make
        {
            get { return _make; }
            set { _make = value; NotifyPropertyChanged(nameof(Make)); }
        }


        public string Model
        {
            get { return _model; }
            set { _model = value; NotifyPropertyChanged(nameof(Model)); }
        }

        public ICommand AddCarCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    try
                    {
                        //add the car to the repo
                        CarRepository.Instance.AddCar(new Models.CarModel()
                        {
                            Make = Make,
                            Model = Model,
                        });

                        //notify that the process is complete, so that the window can close
                        NotifyOnComplete(true);
                    }
                    catch (Exception ex)
                    {
                        //show an the error messagew if there was an exception
                        NotifyErrorOccured(ex);
                    }


                }, (obj) =>
                {
                    //is the button enabled
                    return (!string.IsNullOrWhiteSpace(Model) && !string.IsNullOrWhiteSpace(Make));
                });
            }

        }
    }
}
