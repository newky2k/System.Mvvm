using System;
using System.Collections.Generic;
using System.Text;

namespace System.Mvvm
{
	/// <summary>
	/// Attribute for configuring property validation
	/// </summary>
	/// <seealso cref="System.Attribute" />
	[AttributeUsage(AttributeTargets.Property)]
    public class ValidatedAttribute : Attribute
    {

    }
}
