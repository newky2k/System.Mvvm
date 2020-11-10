using System;
using System.Collections.Generic;
using System.Text;

namespace System.Mvvm.Attributes
{
    [Obsolete("Use MvvmServiceAttribute instead to register UI services")]
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class UIServiceAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceType">Service Type</param>
        public UIServiceAttribute(Type serviceType)
        {
            Implementation = serviceType;
        }

        public Type Implementation { get; private set; }
    }
}
