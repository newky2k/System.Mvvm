using System;
using System.Collections.Generic;
using System.Linq;
using System.Mvvm;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System.Mvvm
{
    public partial class MvvmManager
    {
        private static void PlatformInit()
        {
            DelegateCommand.RequeryCommandsOnChange = true;
        }



    }
}
