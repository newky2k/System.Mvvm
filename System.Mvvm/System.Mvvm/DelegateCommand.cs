using System;
using System.Windows.Input;

namespace System.Mvvm
{
	public class DelegateCommand : ICommand
	{
		public DelegateCommand()
		{

		}

		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter)
		{
			throw new NotImplementedException();
		}

		public void Execute(object parameter)
		{
			throw new NotImplementedException();
		}
	}
}
