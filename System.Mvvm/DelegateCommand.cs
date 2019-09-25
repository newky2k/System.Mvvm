using System;
using System.Windows.Input;

namespace System.Mvvm
{
    /// <summary>
    /// Command Helper class
    /// </summary>
    public class DelegateCommand : ICommand
    {
        #region Static Properties

        public static Action<EventHandler, bool> ExecuteChanged { get; set; }

        #endregion

        #region Fields
        private ExecuteMethod executeMethod;
        private Func<object, bool> canExecute;
        #endregion

        #region Properties
        public delegate void ExecuteMethod();
        #endregion

        #region Events


        public event EventHandler CanExecuteChanged
        {
            add
            {
                ExecuteChanged?.Invoke(value, true);

            }
            remove
            {
                ExecuteChanged?.Invoke(value, false);
            }
        }
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
            executeMethod();
        }

        #endregion


    }
}
