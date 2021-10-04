using System;
using System.Collections.Generic;
using System.Text;

namespace System.Mvvm
{
    public partial class MvvmManager
    {
        private static void PlatformInit() => throw new PlatformNotSupportedException();
     }
}
