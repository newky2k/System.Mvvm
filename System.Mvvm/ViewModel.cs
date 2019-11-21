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
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        /// <summary>
        /// Called when an exception is thrown
        /// </summary>
        public static event EventHandler<Exception> OnErrorOccured = delegate { };

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
        //     entity.
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged = delegate { };

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether [data has changed].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [data has changed]; otherwise, <c>false</c>.
        /// </value>
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
        /// <value>
        ///   <c>true</c> if this instance is loaded; otherwise, <c>false</c>.
        /// </value>
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
        /// <value>
        ///   <c>true</c> if this instance is busy; otherwise, <c>false</c>.
        /// </value>
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
                        NotifyPropertyChanged("IsBusy");
                        NotifyPropertyChanged("IsBusyReversed");
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
        /// <value>
        /// <c>true</c> if IsBusy reversed; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsBusyReversed
        {
            get
            {
                return !IsBusy;
            }
        }

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

        public virtual bool IsValid
        {
            get { return !this.Validator.HasErrors; }

        }

        public virtual bool HasErrors
        {
            get { return (Validator.Errors.Count > 0); }
        }

        public virtual Dictionary<string, List<string>> Errors
        {
            get
            {
                return Validator.Errors;
            }
        }

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
        /// <value>
        /// <c>true</c> if this instance is editable; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsEditable
        {
            get { return _isEditable; }
            set
            {
                _isEditable = value;

                NotifyPropertyChanged("IsEditable");
                NotifyPropertyChanged("IsEditableReversed");
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is editable reversed.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is editable reversed; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsEditableReversed
        {
            get
            {
                return !IsEditable;
            }
        }

        
        #endregion

        #region Constructors
        public ViewModel()
		{

		}

        #endregion

        #region Methods
        /// <summary>
        /// Notifies the property changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null, Boolean hasChanged = true)
        {
            if (hasChanged == true)
                DataHasChanged = true;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

            if (_propertyChangeActions.ContainsKey(propertyName))
                _propertyChangeActions[propertyName]?.Invoke();

            if (DelegateCommand.RequeryCommandsOnChange)
                RequeryCommands();
        }

        /// <summary>
        /// Notify that the specified properties have changed
        /// </summary>
        /// <param name="propertyNames">list of Parameter names</param>
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
        }

        /// <summary>
        /// All Properties Changed (ie the Model changed).
        /// </summary>
        public void NotifyAllPropertiesDidChange()
        {
            DataHasChanged = true;

            PropertyChanged(this, new PropertyChangedEventArgs(String.Empty));

            foreach (var prop in _propertyChangeActions.Keys)
            {
                _propertyChangeActions[prop]?.Invoke();

            }

            if (DelegateCommand.RequeryCommandsOnChange)
                RequeryCommands();
        }


        /// <summary>
        /// Notifies the error occured.
        /// </summary>
        /// <param name="ex">The ex.</param>
        protected void NotifyErrorOccured(Exception ex, string title = null)
        {
            IsBusy = false;

            if (string.IsNullOrWhiteSpace(title))
                OnErrorOccured(this, ex);
            else
                OnErrorOccured(this, new TitledException(title, ex));
        }

        private void NotifyLoadedChanged(bool value)
        {
            OnLoadedChanged(this, value);
        }

        /// <summary>
        /// Notify that the property has changed and validate the new value at the same time
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="hasChanged"></param>
        protected void NotifyAndValidateProperty([CallerMemberName] string propertyName = null, Boolean hasChanged = true)
        {
            NotifyPropertyChanged(propertyName, hasChanged);

            ValidateProperty(propertyName);
        }

        protected void NotifyOnComplete(bool value)
        {
            this.OnComplete?.Invoke(this, value);
        }

        /// <summary>
        /// When the specified property changes execute the specified action
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="propchangedAction">The action to perform</param>
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
        #endregion

        #region Error Handling

        /// <summary>
        /// Gets the validation errors for a specified property or for the entire entity.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public IEnumerable GetErrors(string propertyName)
        {
            return Validator.GetErrors(propertyName);
        }

        /// <summary>
        /// Add a validator
        /// </summary>
        /// <param name="propertyName">The name of the property to validate</param>
        /// <param name="validator">Function returning an error message if validation fails</param>
        public void AddValidator(string propertyName, string errorMessage, Func<bool> validator)
        {
            Validator.AddValidation(propertyName, errorMessage, validator);

        }

        /// <summary>
        /// Remove the validator
        /// </summary>
        /// <param name="propertyName"></param>
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
        /// <param name="message">Error message to show</param>
        /// <param name="validator">validate function</param>
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
        /// <param name="propertyNames">List of property names</param>
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

        private void RequeryCommands()
        {
            var aType = GetType();

            if (aType != null)
            {
                var commands = aType.GetRuntimeProperties().Where(x => x.PropertyType.Equals(typeof(ICommand)));

                if (commands.Any())
                {
                    foreach (var command in commands)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs(command.Name));
                    }
                }
            }
        }

        #endregion
    }
}
