# System.Mvvm.SourceGenerators

Source generator library for [DSoft.System.Mvvm](https://www.nuget.org/packages/DSoft.System.Mvvm)

## Functionality
- MVVMViewModelAttribute
  - Class attribute for converting class to a `ViewModel` and enabling notificaiton of ICommand CanExecute notifications

# MVVMViewModelAttribute

Add the `MVVMViewModel` attribute to your view model class.

The source generator will detect properties that have types of `System.Windows.Input.ICommand` or `System.Mvvm.DelegateCommand` and fields of type `System.Mvvm.DelegateCommand` and will generate overrides for `NotifyCommandsPropertiesChanged` and `NotifyCommandFieldsCanExecuteChanged` to ensure that bindings to the commands are updated.

### Example class


    [MVVMViewModel]
	public partial class SourceGeneratorViewModel
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

### Generated partial

    public partial class SourceGeneratorViewModel : System.Mvvm.ViewModel
    {
        
        protected override void NotifyCommandsPropertiesChanged()
        {
            SimpleNotififyPropertyChanged("OkCommand");
            SimpleNotififyPropertyChanged("DoSomethingCommand");
            SimpleNotififyPropertyChanged("DidSomethingOnce");
        }

        protected override void NotifyCommandFieldsCanExecuteChanged()
        {
            var dCommands = new List<DelegateCommand>();

            if (_cancelCommand != null)
                dCommands.Add(_cancelCommand);

            if (dCommands.Any())
                DelegateCommand.BulkNotifyRaiseCanExecuteChanged(dCommands);
        }
    }
