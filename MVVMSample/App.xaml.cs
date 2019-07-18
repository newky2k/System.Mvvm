using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Mvvm;
using System.Mvvm.Wpf;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MVVMSample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MvvmManager.Init();

        }
    }
}
