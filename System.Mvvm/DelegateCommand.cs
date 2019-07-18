using System;
using System.Windows.Input;

namespace System.Mvvm
{
    public class DelegateCommand : ICommand
    {
        #region Static Properties

        public static Action<EventHandler, bool> ExecuteChanged
        {
            get;
            set;
        }


        #endregion

        #region Fields
        private ExecuteMethod meth;
        private Func<object, bool> mCanExecute;
        #endregion

        #region Properties
        public delegate void ExecuteMethod();
        #endregion

        #region Events


        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (ExecuteChanged != null)
                {
                    ExecuteChanged(value, true);
                }

            }
            remove
            {
                if (ExecuteChanged != null)
                {
                    ExecuteChanged(value, false);
                }
            }
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="exec">The execute.</param>
        public DelegateCommand(ExecuteMethod exec)
        {

            meth = exec;
        }

        public DelegateCommand(ExecuteMethod exec, Func<object, bool> canExecutePredicate)
            : this(exec)
        {
            mCanExecute = canExecutePredicate;
        }
        #endregion

        #region Methods
        public bool CanExecute(object parameter)
        {
            if (mCanExecute == null)
            {
                return true;
            }
            else
            {
                return mCanExecute(parameter);
            }
        }

        public void Execute(object parameter)
        {
            meth();
        }

        #endregion


    }
}
