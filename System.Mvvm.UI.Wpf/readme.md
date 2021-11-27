# System.Mvvm.UI.WPF

Provides WPF (.NET Framework, .NET Core 3.1 and .NET 5.0 and above) platform implementations for [DSoft.System.Mvvm.UI](https://www.nuget.org/packages/DSoft.System.Mvvm.UI)

## Functionality
 - Core UI functions for Showing Alerts and Confirmation Dialogs 
 - UI Thread execution
 - Dependency injection support with `IPlatformCoreUIProvider`
 - Works with (.NET Framework, .NET Core 3.1 and .NET 5.0 and above)
   - Windows 7 and above

## Support for .NET 5.0 and above with Windows 10/11

There are two Target Framework Monikers (TFMs) that are supported for Windows 10 and above when using .NET 5.0 or above, depending on if your application is using the Windows 10/11 SDK.

 - net5.0-windows7.0
   - Works with Windows 7, 8, 8.1, 10 and 11
 - net5.0-windows10.0.18362.0
   - Works with Windows 10 1903 (18362) and above only to access the Windows 10/11 SDK

# Using System.Mvvm.UI.WPF

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

The standard `UI` functions can be called directly after adding the `DSoft.Mvvm.UI` package to your shared code

    using System.Mvvm;
    ... 

    var result = await UI.ShowConfirmationDialogAsync("Confirm this", "This is a platform specific confirmation message");

    if (result)
        await UI.ShowAlertAsync("YAY!", "You confirmed that");

In the WPF application project that contains the `App` class(or other sub-class of `Application`) to the application, add the `DSoft.System.Mvvm.UI.WPF` package.

Call the `MvvmManager.Init` method in the application code, such as `App` constructor.

    using System.Mvvm;
    ... 
    public partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;

            MvvmManager.Init();

        }
    }
  
## Dependency Injection

Instead of using the static `UI` class you can use dependency injection to access the platform implementation of `IPlatformCoreUIProvider` using extensions for `IServiceCollection` provided by the platform specific packages.

After adding `DSoft.System.Mvvm.UI.WPF` to the application project you can register the core UI implementations of  `IPlatformCoreUIProvider` with the ServiceProvider during configuration of the services using the `AddCoreUI` extension method.

    using System.Mvvm;
    ... 
    public partial class App : Application
    {
        public App()
        {
             ServiceHost.Host = new HostBuilder()
             .ConfigureServices(ConfigureServices)
             .Build();
        }

        void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
        {
           services.AddCoreUI();
        }
    }

Once the core UI is registered you can use `IPlatformCoreUIProvider` with dependency injection in class constructors.

    using System.Mvvm;
    ... 
    public class SharedClassToDoThingsIn
    {
       private readonly IPlatformCoreUIProvider _coreUIProvider;

        public SharedClassToDoThingsIn(IPlatformCoreUIProvider coreUIProvider)
        {
            _coreUIProvider = coreUIProvider;
        }

        public async Task SayHello()
        {
            await _coreUIProvider.ShowAlertAsync("Congrats", "You called the ICoreUIProvider instance");
        }
    }

Use can also retrieve the implementation directly from the services provider or `ServiceHost` if you are using [DSoft.System.Mvvm.Hosting](https://www.nuget.org/packages/DSoft.System.Mvvm.Hosting)

    using System.Mvvm;
    ... 
    var _coreUIProvider = ServiceHost.GetRequiredService<IPlatformCoreUIProvider>();

    await _coreUIProvider.ShowAlertAsync("Congrats", "You called the ICoreUIProvider instance");

Using DI instead of the `UI` does not require a call to `MvvmManager.Init` though you do have to call the extension method to register the services.  You can also use both.

## IWPFPlatformUIProvider

`IWPFPlatformUIProvider` is a platform specific interface that the platform implementation of `` implements.  It provides access to some additonal WPF properties.

 - `CuurentApplication`
 - `CurrentDispatcher`
 - `CurrentWindow`
 - `CurrentWindowOfType<T>()`

Use can retrieve the implementation directly from the services provider or `ServiceHost` if you are using [DSoft.System.Mvvm.Hosting](https://www.nuget.org/packages/DSoft.System.Mvvm.Hosting)

    using System.Mvvm;
    ... 

    var _formsUIProvider = ServiceHost.GetRequiredService<IWPFPlatformUIProvider>();

    var currentApp = _formsUIProvider.CurrentApplication;

### Dialog overrides

`IWPFPlatformUIProvider` also provides the ability to override the default dialog implementations.

Two function properties are provided to set your own implementations, such as using Mahapps.

 - `Func<string,string,Task>ShowAlertOverideFunction`
 - `Func<string,string,Task<bool>>ShowConfirmOverideFunction`

 You can set your own implementations by retrieving the `IWPFPlatformUIProvider` instance, either with `UI.Provider<T>` or from the service provicer instance, `SystemHost.GetRequiredService<T>` if you are using [DSoft.System.Mvvm.Hosting](https://www.nuget.org/packages/DSoft.System.Mvvm.Hosting), and then setting the relevant overrides.

Below is an example that override the default behaviour using MahApps.

    using System.Mvvm;
    using MahApps.Metro.Controls;
    using MahApps.Metro.Controls.Dialogs;
    ... 
     public partial class App : Application
    {
        
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MvvmManager.Init();

            await ServiceHost
                .Initialize(ConfigureServices)
                .StartAsync();

            var wpfPlatform = ServiceHost.GetRequiredService<IWPFPlatformUIProvider>();

            wpfPlatform.ShowAlertOverideFunction = ShowAlertWindow;

            wpfPlatform.ShowConfirmOverideFunction = ShowConfirmationDialog;

        }

        public static Task ShowAlertWindow(string title, string message)
        {
            var wpfPlatform = ServiceHost.GetRequiredService<IWPFPlatformUIProvider>();

            var currentWindow = wpfPlatform.CurrentWindow as MetroWindow;

            return DialogManager.ShowMessageAsync(currentWindow, title, message);
        }

        public static Task<bool> ShowConfirmationDialog(string title, string message)
        {
            var tcs = new TaskCompletionSource<bool>();

            var wpfPlatform = ServiceHost.GetRequiredService<IWPFPlatformUIProvider>();

            UI.InvokeOnUIThread(async () =>
            {
                var currentWindow = wpfPlatform.CurrentWindow as MetroWindow;

                var result = await DialogManager.ShowMessageAsync(currentWindow, title, message, MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings()
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No",
                });

                tcs.SetResult((result == MessageDialogResult.Affirmative));
            });
            

            return tcs.Task;
        }
    }

