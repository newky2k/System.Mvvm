using MVVMSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private List<CarModel> _cars;

        public List<CarModel> Cars
        {
            get { return _cars; }
            set { _cars = value; }
        }



        public CarRepository()
        {
            _cars = new List<CarModel>()
            {
                new CarModel() {Make="Ford", Model="Mustang"},
                new CarModel() {Make="Ford", Model="Kuga"},
                new CarModel() {Make="Ford", Model="Focus"},
                new CarModel() {Make="Ford", Model="Mondeo"},
                new CarModel() {Make="BMW", Model="5 Series"},
                new CarModel() {Make="BMW", Model="3 Series"},
                new CarModel() {Make="BMW", Model="i8"},
                new CarModel() {Make="Tesla", Model="Model 3"},
                new CarModel() {Make="Tesla", Model="Model X"},
                new CarModel() {Make="Ferrari", Model="F40"},
                new CarModel() {Make="Ferrari", Model="F50"},
            };
        }
    }
}
