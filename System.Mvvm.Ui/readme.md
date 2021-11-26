# System.Mvvm.Ui

Multi-platform UI Management for WPF (.NET Framework, .NET Core 3.1 and .NET 5.x), UWP and WinUI 3 (Windows Aop SDK 1.0 and above)(Experimental) and Xamarin.Forms for Mobile.

### Functionality

- UI
  - Core UI functions for Showing Alerts and Confirmation Dialogs (using platform specific implementations)
     - UWP and WPF (.NET Framework, .NET Core 3.1, NET 5.0 and above) and Xamarin.Forms 5.x for mobile
  - UI Thread execution

# Version 3.0 - Breaking in Changes

All UI functionality hae been moved from the main `DSoft.System.Mvvm` package too `DSoft.System.Mvvm.UI` and specific platform packages.

The package and assembly name remains `System.Mvvm.UI` but the namespace has changed to `System.Mvvm` instead.

# Using System.Mvvm.UI

The `UI` static class provides four methods

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

**NOTE: The standard UI functions only work when the platform code has been registered using `MvvmManager.Init` on the supported plaforms**

## Platform support

UI platform support is provide through nuget packages for each support platform. Add these to the main application and call `MvvmManager.Init` to register to UI default UI providers.

### Supported platforms

 - [WPF](https://www.nuget.org/packages/DSoft.System.Mvvm.UI.WPF)
   - .NET Framework 4.6.1 and above
   - .NET Core 3.1
   - .NET 5.x and above
     - Windows 10 is supported for both net5.0-windows7 and 10.0.18362.0 target framework monikers (TFMs) incase you want to use the Windows 10 SDK in your WPF app.
 - [UWP/WinUI](https://www.nuget.org/packages/DSoft.System.Mvvm.UI.WinUI)
   - UWP Windows 10 version 1903 (18362) and above
   - WinUI using Windows App SDK 1.0 or above using .NET 5.0 or above
     - Windows 10 version 1903 (18362) and above
 - [Xamarin.Forms](https://www.nuget.org/packages/DSoft.System.Mvvm.UI.Forms)
   - Xamarin.Forms 5.x for .Net Standard 2.0 and above
     - Use on the shared project containing the Xamarin Application not the platform specific mobile apps.

### MvvmManager
`MvvmManager` is a class that is found in the platform packages for `Dsoft.System.Mvvm.UI` and registers the standard UI implementation for each platform.

Call `MvvmManager.Init`to initialise the platform implementation. 