using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.Mvvm
{
    public abstract class Validator
    {
        #region Fields
        private Dictionary<string, Action<string>> _validators = new Dictionary<string, Action<string>>();
        private Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();
        #endregion

        #region Properties

        public bool HasErrors
        {
            get { return (this._errors.Count > 0); }
        }

        public Action<string> NotificationAction = delegate { };

        public Dictionary<string, Action<string>> Validators
        {
            get
            {
                return _validators;
            }
        }

        public Dictionary<string, List<string>> Errors
        {
            get
            {
                return _errors;
            }
        }

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

        public Validator()
        {

        }

        #endregion

        #region Methods

        public IEnumerable GetErrors(string propertyName)
        {
            if (this._errors.ContainsKey(propertyName))
                return this._errors[propertyName];
            return null;
        }

        public void ClearErrors()
        {
            _errors = new Dictionary<string, List<string>>();
            NotifyErrorsChanged(String.Empty);

        }

        public void AddError(string propertyName, string error)
        {
            // Add error to list
            this._errors[propertyName] = new List<string>() { error };
            NotifyErrorsChanged(propertyName);
        }

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

        private void NotifyErrorsChanged(string propertyName)
        {
            NotificationAction?.Invoke(propertyName);
        }

        public void ValidateProperty([CallerMemberName] string propertyName = null)
        {
            if (Validators.ContainsKey(propertyName))
            {
                Validators[propertyName](propertyName);
            }

        }

        public void ValidateProperties(params string[] propertyNames)
        {
            foreach (var propertyName in propertyNames)
            {
                ValidateProperty(propertyName);
            }
        }

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

        public abstract void ValidateItem(object item);
        #endregion

    }

    public class EmptyValidator : Validator
    {
        public override void ValidateItem(object item)
        {

        }
    }

    public abstract class Validator<T> : Validator
    {
        public Validator()
        {
        }

        public override void ValidateItem(object item) => Validate((T)item);

        public abstract void Validate(T item);
    }
}
