using System;
using System.Collections.Generic;
using System.Text;

namespace System.Mvvm.Attributes
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class MvvmServiceAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceType">Service Type</param>
        public MvvmServiceAttribute(Type serviceType)
        {
            Implementation = serviceType;
        }

        public Type Implementation { get; private set; }
    }
}
