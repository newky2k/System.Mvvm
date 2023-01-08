using System;
using System.Collections.Generic;
using System.Text;

namespace System.Mvvm
{
	/// <summary>
	/// PlatformNotSupported Exception.
	/// Implements the <see cref="Exception" />
	/// </summary>
	/// <seealso cref="Exception" />
	public class PlatformNotSupportedException : Exception
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="PlatformNotSupportedException"/> class.
		/// </summary>
		public PlatformNotSupportedException() : base("Platform not supported. Ensure you are using the correct version of the library")
        {

        }
    }
}
