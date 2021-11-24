using System;
using System.Collections.Generic;
using System.Text;

namespace System.Mvvm
{
    public class PlatformNotSupportedException : Exception
    {
        public PlatformNotSupportedException() : base("Platform not supported.  Ensure you are using the correct version of the library")
        {

        }
    }
}
