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
     - UWP and WPF (.NET Framework and .NET Core 3.0)
  - UI Dependency Service for calling platform UI code from shared code (details below) 
- Services
  - Simple Dependency Service container for dependecy injection service management

# Version 3.0 - Breaking in Changes

`Services` and the dependency injection features have been removed and ported to the `DSoft.ServiceRegistra` 2.0 package.  The API is the same.

All UI functionality has been moved to the `DSoft.System.Ui` package.  The API is the same.


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


