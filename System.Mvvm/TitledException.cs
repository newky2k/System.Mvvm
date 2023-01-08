using System;
using System.Collections.Generic;
using System.Text;

namespace System.Mvvm
{
	/// <summary>
	/// Excption class with a tile as well as a message
	/// </summary>
	/// <seealso cref="System.Exception" />
	public class TitledException : Exception
    {
        private string title;

		/// <summary>
		/// Gets the title.
		/// </summary>
		/// <value>The title.</value>
		public string Title
        {
            get { return title; }
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="TitledException"/> class.
		/// </summary>
		/// <param name="title">The title.</param>
		/// <param name="ex">The ex.</param>
		public TitledException(String title, Exception ex) : base(ex.Message, ex)
        {
            this.title = title;
        }

    }
}
