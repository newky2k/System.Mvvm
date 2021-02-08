# System.MMVM

Model View View Model (MVVM) classes with built in Dependency Injection (DI) for all variants of .NET.

Also provides centralised multi-platform UI Management

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

To create a notifiable property create a full prop using the `propfull` snippet and add the a call to `NotifyPropertyChanged`.

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
The `UI` static class provides two features

  - A central interface for calling some simple UI functions
    - `ShowAlert`
    - `ShowConfirmation`
  - A simple UI dependency service manager for calling platform specific UI implementations from shared code

The standaerd `UI` functions can be called directly

    var result = await UI.ShowConfirmationDialogAsync("Confirm this", "This is a platform specific confirmation message");

    if (result)
        await UI.ShowAlertAsync("YAY!", "You confirmed that");

**NOTE: The standard UI functions only work when the platform code has been registered using `MvvmManager` a the supported plafrom**

You can call `UI.Register<T>` to manually register a class or `UI.Init` and register the calling assembly and a list of optional external assemblies.

To get the implementation from `UI` just call the `UI.Get<T>()` method and it will return the first class the inherits from `T`(or is `T`), which will typically be an interface.

For example:

    var customUI = UI.Get<ITestCustomUIProvider>();

    customUI.SayHello();

`UI` will cache the instance the first time a call to `UI.Get<T>()` is made for the type of `T`.  Call the `UI.Get<T>(bool)` overload if you don't want the instance to be cached.

## MvvmManager
`MvvmManager` is a class that is found in the `Dsoft.System.Mvvm.Ui` package and registers the standard UI implementation for `UI` and registers any instances of the `UIServiceAttribute` on the calling or external assemblys(when provided).

There is a method called `Init` on the MvvmManager with and overload that excepts a list of assemblies. 

**NOTE: Only WPF(.NET framework, .NET Core 3.1, .NET 5.x) is supported with `MvvmManager` at the moment**

# Services
`Services` is a simple dependency service manager for calling platform specific service implementations from shared code

You can call `Services.Register<T>` to manually register a class or `Services.Register` to register the calling assembly. `Services` will locate an instances of `MvvmServiceAttribute` within the assemblys and register the implementations types.

Addtional `Services.Register` methods are:

  - `Register(Type[] types)`
    - Registers a array of implementation types
  - `Register(Assembly[] assemblies)`
    - Processes a array of Assemblies and registers any instances of the `MvvmServiceAttribute`
  - `Register<T,T2>`
    - Explicitly registers an interface, `T`, against an implementatio, `T2`


To get the implementation from `Services` just call the `Services.Get<T>()` method and it will return the first type the inherits from `T`(or is `T`), which will typically be an interface.   If you have specified an explicit implementation of an interface the system will return that.

For example:

    var deviceProvider = Services.Get<ITestPlatformDeviceService>();

    var printers = await deviceProvider.GetPrintersAsync();

`Services` will cache the instance the first time a call to `Services.Get<T>()` is made for the type of `T`.  

***Note: See SingletonServiceAtrribute below for cached or singleton instances of the service***

## MvvmServiceAttribute
`MvvmServiceAttribute` is an assembly level attribute that us used to identify Mvvm Services and register them with the system.

If you want `Services.Init` to register the implementation then you need to add something like this

    [assembly: MvvmService(typeof(TestPlatformDeviceService))]

In this case `TestPlatformDeviceService` is the implementation.

`MvvmServiceAttribute` can also be used to explicity registers an interface against an implemenetion type


    [assembly: MvvmService(typeof(ITestPlatformDeviceService), typeof(TestPlatformDeviceService))]

In this case `TestPlatformDeviceService` is the implementation and `ITestPlatformDeviceService` is the interface

## SingletonServiceAttribute

`SingletonServiceAttribute` allows you to identify an implementation as a singleton instance.  

Decorating a clas with this attribute will tel the system to instatiate only a single instance of the implementation.

    [assembly: MvvmService(typeof(ITestCustomUIProvider), typeof(TestCustomUIProvider))]
    namespace MVVMSample.Providers
    {
        [SingletonService]
        public class TestCustomUIProvider : ITestCustomUIProvider

