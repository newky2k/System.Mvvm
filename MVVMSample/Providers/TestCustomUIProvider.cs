﻿using MVVMSample.Contracts;
using MVVMSample.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
