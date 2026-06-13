# System.Mvvm

Model View View Model (MVVM) classes with built in Dependency Injection (DI) for all variants of .NET.

Also provides centralised multi-platform UI Management for WPF (.NET Framework and .NET 10), WinUI 3 and MAUI for calling UI functionality from your Non-UI shared code.

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
     - WinUI, WPF (.NET Framework and .NET 10) and MAUI

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



# UI

`UI` functionality is provided through nuget packages for each supported platform center around the [DSoft.System.Mvvm.UI](https://www.nuget.org/packages/DSoft.System.Mvvm.UI) package, which allows you to access UI functionality from your shared Non-UI code.

### Supported platforms

 - [WPF](https://www.nuget.org/packages/DSoft.System.Mvvm.UI.WPF)
   - .NET Framework 4.6.2 and above
   - .NET 10 and above
     - Windows 10 and above is supported for both net10.0-windows7.0 and net10.0-windows10.0.18362.0 target framework monikers (TFMs) incase you want to use the Windows 10 SDK in your WPF app.
 - [WinUI](https://www.nuget.org/packages/DSoft.System.Mvvm.UI.WinUI)
   - WinUI 3 using Windows App SDK 1.0 or above on .NET 10
     - Windows 10 version 2004 (19041) and above
 - [MAUI](https://www.nuget.org/packages/DSoft.System.Mvvm.UI.MAUI)
   - .NET 10 (net10.0, Android, iOS, MacCatalyst and Windows 10.0.19041.0)
 - [Visual Studio Extensibility](https://www.nuget.org/packages/DSoft.System.Mvvm.VisualStudio.Extensibility.UI)
   - VisualStudio.Extensibility SDK on .NET 8 (Windows)