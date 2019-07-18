using System;
using System.Collections.Generic;
using System.Text;

namespace System.Mvvm
{
    public class TitledException : Exception
    {
        private string title;

        public string Title
        {
            get { return title; }
        }

        public TitledException(String title, Exception ex) : base(ex.Message, ex)
        {
            this.title = title;
        }

    }
}
