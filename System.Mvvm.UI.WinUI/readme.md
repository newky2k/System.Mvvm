# System.Mvvm.UI.WinUI

Provides UWP and Win UI 3 platform implementations for [DSoft.System.Mvvm.UI](https://www.nuget.org/packages/DSoft.System.Mvvm.UI)

### Functionality

- UI
  - Core UI functions for Showing Alerts and Confirmation Dialogs 
  - UI Thread execution

# Using System.Mvvm.UI.WinUI

The `UI` static class provides two features

  - A central interface for calling some simple UI functions
    - `ShowAlert(string title, string message)`  
      - Show an alert with a title and message using the platform implementation
    - `ShowConfirmation(string title, string message)`
      - Show a confirmation dialog and return the result using the platform implementation
    - `InvokeOnUIThread(Action)`
      - Execute the action on the UI thread using the platform implementation
    - `InvokeOnUIThreadAsync(Action)`
      - Execute the action on the UI thread asyncronously using the platform implementation

The standard `UI` functions can be called directly after adding the `DSoft.Mvvm.UI` package to your shared code

    using System.Mvvm;
    ... 

    var result = await UI.ShowConfirmationDialogAsync("Confirm this", "This is a platform specific confirmation message");

    if (result)
        await UI.ShowAlertAsync("YAY!", "You confirmed that");

In the shared UWP or WinUI application project that contains the `App` class(or other sub-class of `Application`) to the application, add the `DSoft.System.Mvvm.UI.WinUI` package.

Call the `MvvmManager.Init` method in the shared code, such as `Application.OnStart`

    using System.Mvvm;
    ... 
    public partial class App : Application
    {
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            base.OnStart();

            MvvmManager.Init();
        }
    }