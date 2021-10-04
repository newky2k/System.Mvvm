# System.Mvvm

Model View View Model (MVVM) classes with built in Dependency Injection (DI) for all variants of .NET.

Also provides centralised multi-platform UI Management for WPF (.NET Framework, .NET Core 3.1 and .NET 5.x) and WinUI 3 (Preview 4 and above)(Experimental)

### Functionality

- Implements INotifyPropertyChanged
- Implements INotifyDataErrorInfo
- Events for handling and detecting changes
- Additional properties for 
  - IsLoaded
  - IsBusy
  - IsBusyReveresed
  - IsValid
  - IsEditable
  - IsEditableReversed
- Data Validation 
- Built-In Error notification methods
- ICommand binding helpers
- Notification extension actions
    - Rather than overriding a property, add an action when it changes
- Base View Models for
    - Forms
    - Lists
    - Searchable Lists
    - Tree View Searchable Lists
- UI
  - Core UI functions for Showing Alerts and Confirmation Dialogs (using platform specific implementations)
     - UWP, WinUI and WPF (.NET Framework and .NET Core 3.1, .NET 5+)

# Classes

## ViewModel
This is the base View Model abstract class which contains the most common MVVM functionality

## ListViewModel<T,T2>
This class inherits from `ViewModel` and provides additonal functionality for managing lists of data

## SearchViewModel<T, T2>
This class inherits from `ListViewModel<T,T2>` and adds search and filtering functionality to the standard ListViewModel

## SearchTreeViewModel<T, T2>
This class inherits from `SearchViewModel<T, List<T>>` and adds Tree Path preparation to the standard SearchViewModel

# Using System.Mvvm

## Basic ViewModel
All of the ViewModel base classess are in the `System.Mvvm` namespace.  To create a basic ViewModel simple inherit from `ViewModel`.

    using System.Mvvm;
    ...
    public class MainViewModel : ViewModel
    {

To create a notifiable property create a property with a backing field using the `propfull` snippet and add a call to `NotifyPropertyChanged`.

    private int myVar;

    public int MyProperty
    {
        get { return myVar; }
        set { myVar = value;  NotifyPropertyChanged();}
    }

`NotifyPropertyChanged` will automatically pickup the calling member, but you can also be explicit.

    private int myVar;

    public int MyProperty
    {
        get { return myVar; }
        set { myVar = value; NotifyPropertyChanged(nameof(MyProperty)); }
    }



# Using System.Mvvm.Ui

The `UI` static class provides two features

  - A central interface for calling some simple UI functions
    - `ShowAlert(string title, string message)`  
      - Show an alert with a title and message
    - `ShowConfirmation(string title, string message)`
      - Show a confirmation dialog and return the result
    - `InvokeOnUIThread(Action)`
      - Execute the action on the UI thread
    - `InvokeOnUIThreadAsync(Action)`
      - Execute the action on the UI thread asyncronously

The standard `UI` functions can be called directly after adding the `DSoft.Mvvm.Ui` package

    using System.Mvvm;
    ... 

    var result = await UI.ShowConfirmationDialogAsync("Confirm this", "This is a platform specific confirmation message");

    if (result)
        await UI.ShowAlertAsync("YAY!", "You confirmed that");

**NOTE: The standard UI functions only work when the platform code has been registered using `MvvmManager.Init` on the supported plaforms**

## MvvmManager
`MvvmManager` is a class that is found in the `Dsoft.System.Mvvm.Ui` package and registers the standard UI implementation for `UI` for each supported platform.

Call `MvvmManager.Init`to initialise the platform implementation. 

**NOTE: Only WPF(.NET framework, .NET Core 3.1, .NET 5.x) and WinUI 3 (Preview 4 and above) and UWP are supported with `MvvmManager` at the moment**

