using System;
using System.ComponentModel;

namespace System.Mvvm
{
	public class ViewModel : INotifyPropertyChanged
	{
        #region Fields
        private bool mDataHasChanged;
        private bool mIsLoaded;
        private bool mIsBusy;
        private bool disableIsBusyChanged;
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

        public event EventHandler<bool> OnLoadedChanged = delegate { };

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
            get { return mDataHasChanged; }
            set
            {
                if (mDataHasChanged != value)
                {
                    mDataHasChanged = value;
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
            get { return mIsLoaded; }
            set
            {
                if (mIsLoaded != value)
                {
                    mIsLoaded = value;

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
            get { return mIsBusy; }
            set
            {
                if (mIsBusy != value)
                {
                    mIsBusy = value;

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
            get { return disableIsBusyChanged; }
            set { disableIsBusyChanged = value; }
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
        protected void NotifyPropertyChanged(string propertyName, Boolean hasChanged = true)
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


    }
}
