using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.Mvvm
{
	/// <summary>
	/// Validation provider class
	/// </summary>
	public abstract class Validator
    {
        #region Fields
        private Dictionary<string, Action<string>> _validators = new Dictionary<string, Action<string>>();
        private Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();
		#endregion

		#region Properties

		/// <summary>
		/// Gets a value indicating whether this instance has errors.
		/// </summary>
		/// <value><c>true</c> if this instance has errors; otherwise, <c>false</c>.</value>
		public bool HasErrors
        {
            get { return (this._errors.Count > 0); }
        }

		/// <summary>
		/// Gets or sets the notification action.
		/// </summary>
		/// <value>The notification action.</value>
		public Action<string> NotificationAction { get; set; }  = delegate { };

		/// <summary>
		/// Gets the validators.
		/// </summary>
		/// <value>The validators.</value>
		public Dictionary<string, Action<string>> Validators
        {
            get
            {
                return _validators;
            }
        }

		/// <summary>
		/// Gets the errors.
		/// </summary>
		/// <value>The errors.</value>
		public Dictionary<string, List<string>> Errors
        {
            get
            {
                return _errors;
            }
        }

		/// <summary>
		/// Gets the error messages.
		/// </summary>
		/// <value>The error messages.</value>
		public string ErrorMessages
        {
            get
            {
                if (!HasErrors)
                    return string.Empty;

                var strBld = new StringBuilder();

                foreach (var propName in Errors.Keys)
                {
                    var lst = Errors[propName];

                    foreach (var aError in lst)
                        strBld.AppendLine(aError);
                }

                return strBld.ToString();

            }
        }

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Validator"/> class.
		/// </summary>
		public Validator()
        {

        }

		#endregion

		#region Methods

		/// <summary>
		/// Gets the errors.
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		/// <returns>IEnumerable.</returns>
		public IEnumerable GetErrors(string propertyName)
        {
            if (this._errors.ContainsKey(propertyName))
                return this._errors[propertyName];
            return null;
        }

		/// <summary>
		/// Clears the errors.
		/// </summary>
		public void ClearErrors()
        {
            _errors = new Dictionary<string, List<string>>();
            NotifyErrorsChanged(String.Empty);

        }

		/// <summary>
		/// Add an error.
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		/// <param name="error">The error.</param>
		public void AddError(string propertyName, string error)
        {
            // Add error to list
            this._errors[propertyName] = new List<string>() { error };
            NotifyErrorsChanged(propertyName);
        }

		/// <summary>
		/// Remove error.
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		public void RemoveError(string propertyName)
        {
            // remove error
            if (this._errors.ContainsKey(propertyName))
                this._errors.Remove(propertyName);

            NotifyErrorsChanged(propertyName);


        }

		/// <summary>
		/// Resets the error. If the validation fails it adds the error with the message, otherwise it clears the error
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		/// <param name="error">The error.</param>
		/// <param name="validation">The validation.</param>
		public void ResetError(string propertyName, string error, Func<bool> validation)
        {
            var result = validation();

            if (result == false)
                AddError(propertyName, error);
            else
                RemoveError(propertyName);

            NotifyErrorsChanged(propertyName);
        }

		/// <summary>
		/// Notifies the errors changed.
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		private void NotifyErrorsChanged(string propertyName)
        {
            NotificationAction?.Invoke(propertyName);
        }

		/// <summary>
		/// Validates the property.
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		public void ValidateProperty([CallerMemberName] string propertyName = null)
        {
            if (Validators.ContainsKey(propertyName))
            {
                Validators[propertyName](propertyName);
            }

        }

		/// <summary>
		/// Validates the properties.
		/// </summary>
		/// <param name="propertyNames">The property names.</param>
		public void ValidateProperties(params string[] propertyNames)
        {
            foreach (var propertyName in propertyNames)
            {
                ValidateProperty(propertyName);
            }
        }

		/// <summary>
		/// Adds the validation.
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		/// <param name="errorMessage">The error message.</param>
		/// <param name="validation">The validation.</param>
		public void AddValidation(string propertyName, string errorMessage, Func<bool> validation)
        {

            if (Validators.ContainsKey(propertyName))
            {
                Validators[propertyName] = (propName) =>
                {
                    ResetError(propName, errorMessage, validation);
                };
            }
            else
            {
                Validators.Add(propertyName, (propName) =>
                {
                    ResetError(propName, errorMessage, validation);
                });
            }
        }

		/// <summary>
		/// Validates the item.
		/// </summary>
		/// <param name="item">The item.</param>
		public abstract void ValidateItem(object item);
        #endregion

    }

	/// <summary>
	/// Empty Validator class
	/// Implements the <see cref="System.Mvvm.Validator" />
	/// </summary>
	/// <seealso cref="System.Mvvm.Validator" />
	public class EmptyValidator : Validator
    {
		/// <summary>
		/// Validates the item.
		/// </summary>
		/// <param name="item">The item.</param>
		public override void ValidateItem(object item)
        {

        }
    }

	/// <summary>
	/// Abstract Validator class with Generic Type support
	/// Implements the <see cref="System.Mvvm.Validator" />
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <seealso cref="System.Mvvm.Validator" />
	public abstract class Validator<T> : Validator
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="Validator{T}"/> class.
		/// </summary>
		public Validator()
        {
        }

		/// <summary>
		/// Validates the item.
		/// </summary>
		/// <param name="item">The item.</param>
		public override void ValidateItem(object item) => Validate((T)item);

		/// <summary>
		/// Validates the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		public abstract void Validate(T item);
    }
}
