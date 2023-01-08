using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace System.Mvvm
{
	/// <summary>
	/// Base view model class for views
	/// </summary>
	/// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
	/// <seealso cref="System.ComponentModel.INotifyDataErrorInfo" />
	public abstract class ViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
	{
        
        #region Fields
        private bool _dataHasChanged;
        private bool _isLoaded;
        private bool _isBusy;
        private bool _isEditable;
        private bool _disableIsBusyChanged;
        private Dictionary<string, Action> _propertyChangeActions = new Dictionary<string, Action>();
        private Validator _validator;
		#endregion

		#region Events
		/// <summary>
		/// Occurs when a property value changes.
		/// </summary>
		/// <returns></returns>
		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		/// <summary>
		/// Called when an exception is thrown
		/// </summary>
		public static event EventHandler<Exception> OnErrorOccurred = delegate { };

		/// <summary>
		/// Occurs when [on is busy changed].
		/// </summary>
		public event EventHandler<bool> OnIsBusyChanged = delegate { };

		/// <summary>
		/// Occurs when then IsLoaded Changes
		/// </summary>
		public event EventHandler<bool> OnLoadedChanged = delegate { };

		/// <summary>
		/// Occurs when the workflow is complete, such as to close a window or view
		/// </summary>
		public event EventHandler<bool> OnComplete = delegate { };

		/// <summary>
		/// Occurs when the validation errors have changed for a property or for the entire
		/// entity.
		/// </summary>
		/// <returns></returns>
		public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged = delegate { };

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets a value indicating whether [data has changed].
		/// </summary>
		/// <value><c>true</c> if [data has changed]; otherwise, <c>false</c>.</value>
		public virtual bool DataHasChanged
        {
            get { return _dataHasChanged; }
            set
            {
                if (_dataHasChanged != value)
                {
                    _dataHasChanged = value;
                }
            }
        }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is loaded.
		/// </summary>
		/// <value><c>true</c> if this instance is loaded; otherwise, <c>false</c>.</value>
		public virtual bool IsLoaded
        {
            get { return _isLoaded; }
            set
            {
                if (_isLoaded != value)
                {
                    _isLoaded = value;

                    NotifyLoadedChanged(value);
                }
            }
        }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is busy.
		/// </summary>
		/// <value><c>true</c> if this instance is busy; otherwise, <c>false</c>.</value>
		public virtual bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;

                    if (!DisableIsBusyChanged)
                    {
                        NotifyPropertiesChanged(nameof(IsBusy), nameof(IsBusyReversed));

                        OnIsBusyChanged(this, value);
                    }

                }

            }
        }

		/// <summary>
		/// Gets or sets value to disable the IsBusyChanged Notification
		/// </summary>
		/// <value><c>true</c> if [disable is busy changed]; otherwise, <c>false</c>.</value>
		public virtual bool DisableIsBusyChanged
        {
            get { return _disableIsBusyChanged; }
            set { _disableIsBusyChanged = value; }
        }

		/// <summary>
		/// Get the reverse of IsBusy
		/// </summary>
		/// <value><c>true</c> if IsBusy reversed; otherwise, <c>false</c>.</value>
		public virtual bool IsBusyReversed
        {
            get
            {
                return !IsBusy;
            }
        }

		/// <summary>
		/// Gets or sets the validator.
		/// </summary>
		/// <value>The validator.</value>
		public virtual Validator Validator
        {
            get
            {
                if (_validator == null)
                {
                    _validator = new EmptyValidator();
                    _validator.NotificationAction = NotifyErrorChanged;
                }

                return _validator;
            }
            set
            {
                _validator = value;
                _validator.NotificationAction = NotifyErrorChanged;
            }
        }

		/// <summary>
		/// Returns true if ... is valid.
		/// </summary>
		/// <value><c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
		public virtual bool IsValid
        {
            get { return !this.Validator.HasErrors; }

        }

		/// <summary>
		/// Gets a value that indicates whether the entity has validation errors.
		/// </summary>
		/// <value><c>true</c> if this instance has errors; otherwise, <c>false</c>.</value>
		public virtual bool HasErrors
        {
            get { return (Validator.Errors.Count > 0); }
        }

		/// <summary>
		/// Gets the errors.
		/// </summary>
		/// <value>The errors.</value>
		public virtual Dictionary<string, List<string>> Errors
        {
            get
            {
                return Validator.Errors;
            }
        }

		/// <summary>
		/// Gets the error messages.
		/// </summary>
		/// <value>The error messages.</value>
		public virtual string ErrorMessages
        {
            get
            {
                return Validator.ErrorMessages;
            }
        }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is editable.
		/// </summary>
		/// <value><c>true</c> if this instance is editable; otherwise, <c>false</c>.</value>
		public virtual bool IsEditable
        {
            get { return _isEditable; }
            set
            {
                _isEditable = value;

                NotifyPropertiesChanged(nameof(IsEditable), nameof(IsEditableReversed));
            }
        }

		/// <summary>
		/// Gets a value indicating whether this instance is editable reversed.
		/// </summary>
		/// <value><c>true</c> if this instance is editable reversed; otherwise, <c>false</c>.</value>
		public virtual bool IsEditableReversed
        {
            get
            {
                return !IsEditable;
            }
        }


		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="ViewModel"/> class.
		/// </summary>
		public ViewModel()
		{

		}

		#endregion

		#region Methods

		#region Property Change Notification Methods

		/// <summary>
		/// Call to notify that a property has changed
		/// </summary>
		/// <param name="propertyName">Name of the property that has changed</param>
		/// <param name="hasChanged">if set to <c>true</c> [has changed].</param>
		protected void OnPropertyChanged([CallerMemberName] string propertyName = null, bool hasChanged = true) => NotifyPropertyChanged(propertyName, hasChanged);

		/// <summary>
		/// Called when [properties changed].
		/// </summary>
		/// <param name="propertyNames">The property names that have changed</param>
		protected void OnPropertiesChanged(params string[] propertyNames) => NotifyPropertiesChanged(propertyNames);

		/// <summary>
		/// Notifies the property changed.
		/// </summary>
		/// <param name="propertyName">Name of the property that has changed</param>
		/// <param name="hasChanged">if set to <c>true</c> [has changed].</param>
		protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null, bool hasChanged = true)
        {
            if (hasChanged == true)
                DataHasChanged = true;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

            if (_propertyChangeActions.ContainsKey(propertyName))
                _propertyChangeActions[propertyName]?.Invoke();

            if (DelegateCommand.RequeryCommandsOnChange)
                RequeryCommands();
            else if (DelegateCommand.UpdateICommandFields)
                NotifyCommandFieldsCanExecuteChanged();
        }

		/// <summary>
		/// Notify that the specified properties have changed
		/// </summary>
		/// <param name="propertyNames">The property names that have changed</param>
		protected void NotifyPropertiesChanged(params string[] propertyNames)
        {
            if (propertyNames == null || propertyNames.Length == 0)
            {
                NotifyAllPropertiesDidChange();
                return;
            }
                
            foreach (var propertyName in propertyNames)
            {

                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

                if (_propertyChangeActions.ContainsKey(propertyName))
                    _propertyChangeActions[propertyName]?.Invoke();
            }


            if (DelegateCommand.RequeryCommandsOnChange)
                RequeryCommands();
            else if (DelegateCommand.UpdateICommandFields)
                NotifyCommandFieldsCanExecuteChanged();
        }

		/// <summary>
		/// All Properties Changed (ie the Model changed).
		/// </summary>
		public void NotifyAllPropertiesDidChange()
        {
            DataHasChanged = true;

            PropertyChanged(this, new PropertyChangedEventArgs(string.Empty));

            foreach (var prop in _propertyChangeActions.Keys)
            {
                _propertyChangeActions[prop]?.Invoke();

            }

            if (DelegateCommand.RequeryCommandsOnChange)
                RequeryCommands();
            else if (DelegateCommand.UpdateICommandFields)
                NotifyCommandFieldsCanExecuteChanged();
        }

		/// <summary>
		/// When the specified property changes execute the specified action
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		/// <param name="actionToPerform">The action to perform.</param>
		protected void WhenPropertyChanged(string propertyName, Action actionToPerform)
        {
            if (_propertyChangeActions.ContainsKey(propertyName))
            {
                if (actionToPerform != null)
                {
                    _propertyChangeActions[propertyName] = actionToPerform;
                }
                else
                {
                    _propertyChangeActions.Remove(propertyName);
                }
            }
            else
            {
                if (actionToPerform != null)
                {
                    _propertyChangeActions.Add(propertyName, actionToPerform);
                }

            }
        }

		/// <summary>
		/// Notify that the property has changed and validate the new value at the same time
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		/// <param name="hasChanged">The has changed.</param>
		protected void NotifyAndValidateProperty([CallerMemberName] string propertyName = null, Boolean hasChanged = true)
        {
            NotifyPropertyChanged(propertyName, hasChanged);

            ValidateProperty(propertyName);
        }

		#endregion

		#region Event Methods

		/// <summary>
		/// Notifies the loaded changed.
		/// </summary>
		/// <param name="value">if set to <c>true</c> [value].</param>
		private void NotifyLoadedChanged(bool value)
        {
            OnLoadedChanged(this, value);
        }


		/// <summary>
		/// Notifies that the process has completed.  Call the OnComplete event handlers
		/// </summary>
		/// <param name="result">if set to <c>true</c> [result].</param>
		protected void NotifyOnComplete(bool result)
        {
            this.OnComplete?.Invoke(this, result);
        }

		#endregion


		#region Value Update Methods

		/// <summary>
		/// Updates the value and notifies that the property has changed
		/// </summary>
		/// <param name="action">The update action to perform</param>
		/// <param name="propertyName">The name of the property that has changed</param>
		public void UpdateValueAndNotify(Action action, [CallerMemberName] string propertyName = null)
        {
            action();

            NotifyPropertyChanged(propertyName);
        }

		/// <summary>
		/// Updates the value and notifies that the property has changed, if the validator returns true
		/// </summary>
		/// <param name="action">The update action to perform</param>
		/// <param name="validator">The validation function to be evaluated.</param>
		/// <param name="propertyName">The name of the property that has changed</param>
		public void UpdateValueAndNotify(Action action, Func<bool> validator, [CallerMemberName] string propertyName = null)
        {
            if (validator == null)
                UpdateValueAndNotify(action, propertyName);

            if (validator() == true)
            {
                action();

                NotifyPropertyChanged(propertyName);
            }
            
        }

		/// <summary>
		/// Updates the value and notifies that the specified properties have changed
		/// </summary>
		/// <param name="action">The update action to perform</param>
		/// <param name="propertyNames">The property names that have changed</param>
		public void UpdateValueAndNotify(Action action, params string[] propertyNames)
        {
            action();

            NotifyPropertiesChanged(propertyNames);
        }

		/// <summary>
		/// Updates the value and notifies that the specified properties have changed, if the validator returns true
		/// </summary>
		/// <param name="action">The update action to perform</param>
		/// <param name="validator">The validation function to be evaluated.</param>
		/// <param name="propertyNames">The property names that have changed</param>
		public void UpdateValueAndNotify(Action action, Func<bool> validator, params string[] propertyNames)
        {
            if (validator == null)
                UpdateValueAndNotify(action, propertyNames);

            if (validator() == true)
            {
                action();

                NotifyPropertiesChanged(propertyNames);
            }

        }

		#endregion

		#region Error Notifications

		/// <summary>
		/// Notifies that an error occurred.
		/// </summary>
		/// <param name="ex">The ex.</param>
		/// <param name="title">The title.</param>
		protected void NotifyErrorOccurred(Exception ex, string title = null)
        {
            IsBusy = false;

            if (string.IsNullOrWhiteSpace(title))
            {
                OnErrorOccurred(this, ex);
            } 
            else
            {
                OnErrorOccurred(this, new TitledException(title, ex));
            }
        }
		#endregion
		#endregion

		#region Error Handling

		/// <summary>
		/// Gets the validation errors for a specified property or for the entire entity.
		/// </summary>
		/// <param name="propertyName">The name of the property to retrieve validation errors for; or null or <see cref="F:System.String.Empty"></see>, to retrieve entity-level errors.</param>
		/// <returns>The validation errors for the property or entity.</returns>
		public IEnumerable GetErrors(string propertyName)
        {
            return Validator.GetErrors(propertyName);
        }

		/// <summary>
		/// Add a validator
		/// </summary>
		/// <param name="propertyName">The name of the property to validate</param>
		/// <param name="errorMessage">The error message.</param>
		/// <param name="validator">Function returning an error message if validation fails</param>
		public void AddValidator(string propertyName, string errorMessage, Func<bool> validator)
        {
            Validator.AddValidation(propertyName, errorMessage, validator);

        }

		/// <summary>
		/// Remove the validator
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		public void RemoveValidator(string propertyName)
        {

            if (Validator.Validators.ContainsKey(propertyName))
            {
                Validator.Validators.Remove(propertyName);
                
            }

            if (Errors.ContainsKey(propertyName))
                Errors.Remove(propertyName);

        }

		/// <summary>
		/// Validate the property
		/// </summary>
		/// <param name="propertyName">property name</param>
		public void ValidateProperty([CallerMemberName] string propertyName = null)
        {
            Validator.ValidateProperty(propertyName);
        }

		/// <summary>
		/// This will validate all properties with registered validators
		/// </summary>
		public void ValidateAllProperties()
        {
            var properties = Validator.Validators.Keys.ToList();

            foreach (var aProp in properties)
                ValidateProperty(aProp);

        }


		/// <summary>
		/// Notifies the error changed.
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		private void NotifyErrorChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

		/// <summary>
		/// Validate the ViewModel
		/// </summary>
		public void Validate()
        {
            Errors.Clear();

            DataHasChanged = true;
            NotifyAllPropertiesDidChange();

            var props = this.GetType().GetRuntimeProperties().Where(prop => prop.GetCustomAttributes<ValidatedAttribute>().Count() > 0);

            var propNames = props.Select(x => x.Name).ToList();

            foreach (var aProp in propNames)
            {
                Validator.ValidateProperty(aProp);
            }

        }

		/// <summary>
		/// Validate the selected properties
		/// </summary>
		/// <param name="properties">List of property names</param>
		public void Validate(IEnumerable<string> properties)
        {
            Validate(properties.ToArray());
        }

		/// <summary>
		/// Validate the selected properties
		/// </summary>
		/// <param name="properties">The properties.</param>
		public void Validate(params string[] properties)
        {
            Errors.Clear();
            DataHasChanged = true;
            NotifyAllPropertiesDidChange();

            foreach (var aProp in properties)
            {
                Validator.ValidateProperty(aProp);

            }
        }
		#endregion

		#region Private

		/// <summary>
		/// Requeries the commands.
		/// </summary>
		private void RequeryCommands()
        {
            NotifyCommandsPropertiesChanged();

            NotifyCommandFieldsCanExecuteChanged();
        }

		/// <summary>
		/// Simples the notifify property changed.
		/// </summary>
		/// <param name="propName">Name of the property.</param>
		protected void SimpleNotififyPropertyChanged(string propName) => PropertyChanged(this, new PropertyChangedEventArgs(propName));

		/// <summary>
		/// Notifies the commands properties changed.
		/// </summary>
		protected virtual void NotifyCommandsPropertiesChanged()
        {
            var aType = GetType();

            if (aType == null)
                return;

            var commands = aType.GetRuntimeProperties().Where(x => x.PropertyType.Equals(typeof(ICommand)));

            if (commands.Any())
            {
                foreach (var command in commands)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(command.Name));
                }
            }
        }

		/// <summary>
		/// Notifies the command fields can execute changed.
		/// </summary>
		protected virtual void NotifyCommandFieldsCanExecuteChanged()
        {
            var aType = GetType();

            if (aType == null)
                return;

            var commandsField = aType.GetRuntimeFields().Where(x => x.FieldType.Equals(typeof(ICommand))).ToList();

            if (commandsField.Any())
            {
                var dCommands = new List<DelegateCommand>();

                foreach (var command in commandsField)
                {
                    var actualObject = command.GetValue(this) as DelegateCommand;

                    if (actualObject != null)
                        dCommands.Add(actualObject);
                }

                //notify in one go
                if (dCommands.Any())
                    DelegateCommand.BulkNotifyRaiseCanExecuteChanged(dCommands);
            }
        }
        #endregion
    }
}
