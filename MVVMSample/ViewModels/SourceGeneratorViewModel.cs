using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Mvvm;
using System.Windows.Input;

namespace MVVMSample.ViewModels
{
	[MVVMViewModel]
	public partial class SourceGeneratorViewModel
	{

		private DelegateCommand _okCommand;

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

	}

}
