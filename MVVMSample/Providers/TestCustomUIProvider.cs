using MVVMSample.Contracts;
using MVVMSample.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Mvvm.Attributes;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

[assembly: MvvmService(typeof(ITestCustomUIProvider), typeof(TestCustomUIProvider))]
namespace MVVMSample.Providers
{
    [SingletonService]
    public class TestCustomUIProvider : ITestCustomUIProvider
    {
        public void SayHello()
        {
            MessageBox.Show("Hello");
        }
    }
}
