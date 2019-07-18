using MVVMSample.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Mvvm;
using System.Text;
using System.Threading.Tasks;

namespace MVVMSample.Repository
{
    public class CarRepository
    {
        private static Lazy<CarRepository> _instance = new Lazy<CarRepository>(()=> new CarRepository());

        public static CarRepository Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        private ObservableCollection<CarModel> _cars;

        public ObservableCollection<CarModel> Cars
        {
            get { return _cars; }
            set { _cars = value; }
        }



        public CarRepository()
        {
            _cars = new ObservableCollection<CarModel>()
            {
                new CarModel() {Make="Ford", Model="Mustang", Year="1968"},
                new CarModel() {Make="Ford", Model="Mustang",Year="1978"},
                new CarModel() {Make="Ford", Model="Kuga", Year="1968"},
                new CarModel() {Make="Ford", Model="Focus", Year="1968"},
                new CarModel() {Make="Ford", Model="Mondeo", Year="1968"},
                new CarModel() {Make="BMW", Model="5 Series", Year="1968"},
                new CarModel() {Make="BMW", Model="3 Series", Year="1968"},
                new CarModel() {Make="BMW", Model="i8", Year="1968"},
                new CarModel() {Make="Tesla", Model="Model 3", Year="1968"},
                new CarModel() {Make="Tesla", Model="Model X", Year="1968"},
                new CarModel() {Make="Ferrari", Model="F40", Year="1968"},
                new CarModel() {Make="Ferrari", Model="F50", Year="1968"},
            };
        }

        public void AddCar(CarModel car)
        {
            var existCar = Cars.Where(x => x.Make.Equals(car.Make, StringComparison.OrdinalIgnoreCase) && x.Model.Equals(car.Model, StringComparison.OrdinalIgnoreCase));

            if (existCar.Any())
                throw new TitledException("Add Car Failed", new Exception($"{car.Make} {car.Model} already exists"));

            Cars.Add(car);
        }
    }
}
