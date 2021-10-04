# System.Mvvm.Ui

Multi-platform UI Management for WPF (.NET Framework, .NET Core 3.1 and .NET 5.x), UWP and WinUI 3 (Preview 4 and above)(Experimental).



### Functionality

- UI
  - Core UI functions for Showing Alerts and Confirmation Dialogs (using platform specific implementations)
     - UWP and WPF (.NET Framework and .NET Core 3.0)
  - UI Thread execution

# Version 3.0 - Breaking in Changes

All UI functionality hae been moved from the main `DSoft.System.Mvvm` package too `DSoft.System.Mvvm.Ui`

The package and assembly name remains `System.Mvvm.Ui` but the namespace has changed to `System.Mvvm` instead.



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

