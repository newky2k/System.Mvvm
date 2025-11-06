using Microsoft.VisualStudio.Extensibility.UI;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace System.Mvvm;

/// <summary>
/// Base view model class for views
/// </summary>
public class ViewModel : NotifyPropertyChangedObject
{
    #region Fields
    private bool _dataHasChanged;
    private bool _isLoaded;
    private bool _isBusy;
    private bool _isEditable;
    private bool _disableIsBusyChanged;
    private Dictionary<string, Action> _propertyChangeActions = new Dictionary<string, Action>();
    private string _status = string.Empty;
    #endregion

    #region Events

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

    /// <summary>
    /// Gets or sets the status.
    /// </summary>
    /// <value>The status.</value>
    public string Status
    {
        get { return _status; }
        set { _status = value; NotifyPropertyChanged(nameof(Status)); }
    }

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
    #endregion

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

        RaiseNotifyPropertyChangedEvent(propertyName);
    }

    /// <summary>
    /// Notify that the specified properties have changed
    /// </summary>
    /// <param name="propertyNames">The property names that have changed</param>
    protected void NotifyPropertiesChanged(params string[] propertyNames)
    {
        if (propertyNames == null)
        {
            return;
        }

        foreach (var propertyName in propertyNames)
        {

            RaiseNotifyPropertyChangedEvent(propertyName);

        }

    }

    #endregion
}