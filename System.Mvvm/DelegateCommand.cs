using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace System.Mvvm
{
    /// <summary>
    /// Command Helper class
    /// </summary>
    public class DelegateCommand : ICommand
    {
        #region Static Properties

        public static Action<Action> RunOnUiThreadAction {get; set;} = delegate {};

        public static Action<EventHandler, bool> ExecuteChanged { get; set; }

        /// <summary>
        /// If set, the ViewModels will requery an ICommand properties when NotifyPropertyChanged is set
        /// </summary>
        public static bool RequeryCommandsOnChange { get; set; } = true;

        #endregion

        #region Fields
        private ExecuteMethod executeMethod;
        private ExecuteMethodWithParameter executeMethodWithParam;
        private Func<object, bool> canExecute;
        #endregion

        #region Properties
        public delegate void ExecuteMethod();
        public delegate void ExecuteMethodWithParameter(object parameter);
        #endregion

        #region Events


        public event EventHandler CanExecuteChanged;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="exec">The execute method</param>
        public DelegateCommand(ExecuteMethod exec)
        {
            executeMethod = exec;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="exec">The execute method that takes a parameter</param>
        public DelegateCommand(ExecuteMethodWithParameter exec)
        {
            executeMethodWithParam = exec;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="exec">The execute method</param>
        /// <param name="canExecutePredicate">Predicate Function with object parameter</param>
        public DelegateCommand(ExecuteMethod exec, Func<object, bool> canExecutePredicate)
            : this(exec)
        {
            canExecute = canExecutePredicate;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="exec">The execute method that takes a parameter</param>
        /// <param name="canExecutePredicate">Predicate Function with object parameter</param>
        public DelegateCommand(ExecuteMethodWithParameter exec, Func<object, bool> canExecutePredicate)
            : this(exec)
        {
            canExecute = canExecutePredicate;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="exec">The execute method</param>
        /// <param name="canExecutePredicate">Predicate Function without object parameter</param>
        public DelegateCommand(ExecuteMethod exec, Func<bool> canExecutePredicate)
            : this(exec)
        {
            canExecute = (obj) =>
            {
                return canExecutePredicate.Invoke();
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="exec">The execute method</param>
        /// <param name="canExecutePredicate">Predicate Function without object parameter</param>
        public DelegateCommand(ExecuteMethodWithParameter exec, Func<bool> canExecutePredicate)
            : this(exec)
        {
            canExecute = (obj) =>
            {
                return canExecutePredicate.Invoke();
            };
        }

        #endregion

        #region Methods

        public bool CanExecute(object parameter)
        {
            if (canExecute == null)
            {
                return true;
            }
            else
            {
                return canExecute(parameter);
            }
        }

        public void Execute(object parameter)
        {
            if (executeMethod != null)
                executeMethod();
            else if (executeMethodWithParam != null)
                executeMethodWithParam(parameter);
        }

        public void RaiseCanExecuteChanged(bool onUIThread = true)
        {
            if (onUIThread == true)
            {
                RunOnUiThreadAction?.Invoke(() =>
                {
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                });
            }
            else
            {
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }

            //
        }
        #endregion

        #region Static Methods

        public static void BulkNotifyRaiseCanExecuteChanged(IEnumerable<DelegateCommand> commands)
        {
            //update all on the ui in a single call to the UI
            RunOnUiThreadAction?.Invoke(() =>
            {
                foreach (var command in commands) 
                    command.RaiseCanExecuteChanged(false);
            });
        }
        #endregion

    }
}
