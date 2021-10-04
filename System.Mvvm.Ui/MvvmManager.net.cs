using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace System.Mvvm
{
    public partial class MvvmManager
    {
        private static void PlatformInit()
        {
            DelegateCommand.ExecuteChanged = (ev, shoudlAdd) =>
            {
                if (shoudlAdd)
                {
                    CommandManager.RequerySuggested += ev;
                }
                else
                {
                    CommandManager.RequerySuggested -= ev;
                }
            };
        }


    }
}
