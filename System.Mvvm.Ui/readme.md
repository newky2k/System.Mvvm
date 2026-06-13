# System.Mvvm.UI

Multi-platform UI Management for WPF (.NET Framework and .NET 10), WinUI 3 (Windows App SDK 1.0 and above) and MAUI for Mobile.

### Functionality

- UI
  - Core UI functions for Showing Alerts and Confirmation Dialogs (using platform specific implementations)
     - WPF (.NET Framework and .NET 10), WinUI and MAUI for mobile
  - UI Thread execution
  - Dependency injection support with `IPlatformCoreUIProvider`
  - .NET Standard 2.0 and .NET 10 with no dependecies other than `DSoft.System.Mvvm` so it can be used in shared code projects.

# Version 3.0 - Breaking in Changes

All UI functionality has been moved from the main `DSoft.System.Mvvm` package too `DSoft.System.Mvvm.UI` and specific platform packages.

The package and assembly name remains `System.Mvvm.UI` but the namespace has changed to `System.Mvvm` instead.

# Using System.Mvvm.UI

The `UI` static class provides four methods as defined in the `IPlatformCoreUIProvider` interface.  

  - A central interface for calling some simple UI functions
    - `ShowAlert(string title, string message)`  
      - Show an alert with a title and message using the platform implementation
    - `ShowConfirmation(string title, string message)`
      - Show a confirmation dialog and return the result using the platform implementation
    - `InvokeOnUIThread(Action)`
      - Execute the action on the UI thread using the platform implementation
    - `InvokeOnUIThreadAsync(Action)`
      - Execute the action on the UI thread asyncronously using the platform implementation

The standard `UI` functions can be called directly after adding the `DSoft.System.Mvvm.UI` package to your shared code.

    using System.Mvvm;
    ... 

    var result = await UI.ShowConfirmationDialogAsync("Confirm this", "This is a platform specific confirmation message");

    if (result)
        await UI.ShowAlertAsync("YAY!", "You confirmed that");

**NOTE: The standard `UI` functions only work when the platform code has been registered using `MvvmManager.Init` on the supported plaforms**

## Dependency Injection

Instead of using the static `UI` class you can use dependency injection to access the platform implementation of `IPlatformCoreUIProvider` using extensions for `IServiceCollection` provided by the platform specific packages.

Using DI instead of the `UI` does not require a call to `MvvmManager.Init` though you do have to call the extension method to register the services.  See the platform package documentation for more details.  You can also do both.

## Platform support

`UI` platform support is provided through nuget packages for each supported platform. Add these to the main application and call `MvvmManager.Init` to register to UI default UI providers.

### Supported platforms

 - [WPF](https://www.nuget.org/packages/DSoft.System.Mvvm.UI.WPF)
   - .NET Framework 4.6.2 and above
   - .NET 10 and above
     - Windows 10 is supported for both net10.0-windows7.0 and net10.0-windows10.0.18362.0 target framework monikers (TFMs) incase you want to use the Windows 10 SDK in your WPF app.
 - [WinUI](https://www.nuget.org/packages/DSoft.System.Mvvm.UI.WinUI)
   - WinUI using Windows App SDK 1.0 or above on .NET 10
     - Windows 10 version 2004 (19041) and above
 - [MAUI](https://www.nuget.org/packages/DSoft.System.Mvvm.UI.MAUI)
   - .NET 10 (net10.0, Android, iOS, MacCatalyst and Windows 10.0.19041.0)

### MvvmManager
`MvvmManager` is a class that is found in the platform packages for `Dsoft.System.Mvvm.UI` and registers the standard UI implementation for each platform.

Call `MvvmManager.Init` to initialise the platform implementation. 