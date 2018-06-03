using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace System.Mvvm
{
	public class ViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
	{
        #region Fields
        private bool _dataHasChanged;
        private bool _isLoaded;
        private bool _isBusy;
        private bool _disableIsBusyChanged;
        private Dictionary<string, string> _errors = new Dictionary<string, string>();
        private Dictionary<string, Func<string>> _validators = new Dictionary<string, Func<string>>();

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
        public bool DataHasChanged
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
        public bool IsLoaded
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
        public bool IsBusy
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
        public bool DisableIsBusyChanged
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
        public bool IsBusyReversed
        {
            get
            {
                return !IsBusy;
            }
        }



        #endregion

        public ViewModel()
		{

		}

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
        }

        /// <summary>
        /// All Properties Changed (ie the Model changed).
        /// </summary>
        /// <param name="Name">The name.</param>
        public void NotifyAllPropertiesDidChange()
        {
            DataHasChanged = true;

            PropertyChanged(this, new PropertyChangedEventArgs(String.Empty));
        }


        /// <summary>
        /// Notifies the error occured.
        /// </summary>
        /// <param name="ex">The ex.</param>
        protected void NotifyErrorOccured(Exception ex, string title = null)
        {
            OnErrorOccured(this, ex);
        }

        private void NotifyLoadedChanged(bool value)
        {
            OnLoadedChanged(this, value);
        }


        #endregion

        #region Error Handling

        /// <summary>
        /// Gets a value that indicates whether the entity has validation errors.
        /// </summary>
        public bool HasErrors => _errors.Count > 0;

        /// <summary>
        /// Gets the validation errors for a specified property or for the entire entity.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public IEnumerable GetErrors(string propertyName)
        {
            if (_errors.ContainsKey(propertyName))
            {
                return _errors[propertyName];
            }

            return string.Empty;
        }


        public void AddValidator(string propertyName, Func<string> validator)
        {
            if (_validators.ContainsKey(propertyName))
            {
                _validators[propertyName] = validator;
            }
            else
            {
                _validators.Add(propertyName, validator);
            }
        }

        public void RemoveValidator(string propertyName)
        {
            if (_validators.ContainsKey(propertyName))
            {
                _validators.Remove(propertyName);
                
            }

            if (_errors.ContainsKey(propertyName))
                _errors.Remove(propertyName);

        }


        /// <summary>
        /// Validate the property
        /// </summary>
        /// <param name="propertyName">property name</param>
        /// <param name="message">Error message to show</param>
        /// <param name="validator">validate function</param>
        public void ValidateProperty([CallerMemberName] string propertyName = null)
        {
            //if there is no validator then ignore
            if (!_validators.ContainsKey(propertyName))
                return;

            var validator = _validators[propertyName];

            var result = validator();

            if (string.IsNullOrWhiteSpace(result))
            { 
                if (_errors.ContainsKey(propertyName))
                {
                    _errors.Remove(propertyName);

                    NotifyErrorChanged(propertyName);
                }

            }
            else
            {
                if (!_errors.ContainsKey(propertyName))
                {
                    _errors[propertyName] = result;

                    NotifyErrorChanged(propertyName);
                }
            }

        }

        /// <summary>
        /// This will validate all properties with registered validators
        /// </summary>
        public void ValidateAllProperties()
        {
            var properties = _validators.Keys.ToList();

            properties.ForEach(x => ValidateProperty(x));

        }
        public void NotifyErrorChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
        #endregion


    }
}
