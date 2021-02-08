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
        public MvvmServiceAttribute(Type implementationType)
        {
            Implementation = implementationType;
        }

        public MvvmServiceAttribute(Type interfaceType, Type implementationType)
        {
            Interface = interfaceType;
            Implementation = implementationType;
        }

        public Type Interface { get; private set; }

        public Type Implementation { get; private set; }
    }
}
