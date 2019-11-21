using MVVMSample.Contracts;
using MVVMSample.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Mvvm.Attributes;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

[assembly: UIService(typeof(TestCustomUIProvider))]
namespace MVVMSample.Providers
{
    public class TestCustomUIProvider : ITestCustomUIProvider
    {
        public void SayHello()
        {
            MessageBox.Show("Hello");
        }
    }
}
