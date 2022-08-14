using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Mvvm;
using System.Windows.Input;

namespace MVVMSample.ViewModels
{
	//[MVVMViewModel]
	public partial class SourceGeneratorViewModel : ViewModel
	{

		private DelegateCommand _okCommand;
		private ICommand _didCommand;
		private DelegateCommand _cancelCommand;

		public ICommand OkCommand
		{
			get
			{
				return _okCommand ??= new DelegateCommand(() =>
				{
					try
					{

					}
					catch (Exception ex)
					{
						NotifyErrorOccurred(ex);
					}
				});
			}
		}



		public ICommand DoSomethingCommand
		{
			get
			{
				return new DelegateCommand(() =>
				{
					try
					{

					}
					catch (Exception ex)
					{
#if DEBUG
						//this.IsDebug = true;
#endif
						NotifyErrorOccurred(ex);
					}
				});
			}
		}



		public ICommand DidSomethingOnce
		{
			get
			{
				return _didCommand ??= new DelegateCommand(() =>
				{
					try
					{

					}
					catch (Exception ex)
					{

						NotifyErrorOccurred(ex);
					}
				});
			}
		}



	}

}
