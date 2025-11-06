# System.Mvvm

Model View View Model (MVVM) classes with built in Dependency Injection (DI) for all variants of .NET.

Also provides centralised multi-platform UI Management for WPF (.NET Framework, .NET Core 3.1 and .NET 6.x), WinUI 3, UWP and Xamarin.Forms for calling UI functionality from your Non-UI shared code.

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
     - UWP, WinUI and WPF (.NET Framework and .NET Core 3.1, .NET 6+) and Xamarin.Forms

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
   - .NET Framework 4.6.1 and above
   - .NET Core 3.1
   - .NET 6.x and above
     - Windows 10 and above is supported for both net6.0-windows7 and net6.0-windows10.0.18362.0 target framework monikers (TFMs) incase you want to use the Windows 10 SDK in your WPF app.
 - [UWP/WinUI](https://www.nuget.org/packages/DSoft.System.Mvvm.UI.WinUI)
   - UWP Windows 10 version 2004 (19041) and above
   - WinUI using Windows App SDK 1.0 or above using .NET 6.0 or above
     - Windows 10 version 2004 (19041) and above
 - [Xamarin.Forms](https://www.nuget.org/packages/DSoft.System.Mvvm.UI.Forms)
   - Xamarin.Forms 5.x for .Net Standard 2.0 and above
     - Use on the shared project containing the Xamarin Application not the platform specific mobile apps.