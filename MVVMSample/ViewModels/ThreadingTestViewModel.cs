using System;
using System.Collections.Generic;
using System.Mvvm;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVMSample.ViewModels
{
	public class ThreadingTestViewModel : ViewModel
	{

		private bool _isRunning;

		private ICommand _runCommand = null;
		public ICommand RunCommand
		{
			get
			{
				return _runCommand ??= new DelegateCommand(() =>
				{
					try
					{
						UI.ShowAlertAsync("Hi", "World!");

					}
					catch (Exception ex)
					{

						NotifyErrorOccurred(ex);
					}
				},() =>
				{
					return !_isRunning;
				});
			}
		}

		public ThreadingTestViewModel()
		{
			_isRunning = true;
		}

		public async Task LoadAsync()
		{
			IsBusy = true;

			await Task.Delay(5000);

			IsBusy = false;

			_isRunning = false;

			NotifyAllPropertiesDidChange();

			//UI.InvokeOnUIThread(() =>
			//{
			//	((DelegateCommand)_runCommand).RaiseCanExecuteChanged();
			//});
			
		}

	}
}
