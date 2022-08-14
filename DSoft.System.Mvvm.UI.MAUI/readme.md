# System.Mvvm.UI.MAUI

Provides MAUI platform implementations for [DSoft.System.Mvvm.UI](https://www.nuget.org/packages/DSoft.System.Mvvm.UI)

### Functionality

- UI
  - Core UI functions for Showing Alerts and Confirmation Dialogs 
  - UI Thread execution
  - Dependency injection support with `IPlatformCoreUIProvider`

# Using System.Mvvm.UI.MAUI

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

In the shared Xamarin.Forms project that contains the `App` class(or other sub-class of `Application`), add  the `DSoft.System.Mvvm.UI.Forms` package.

***Note: do not add System.Mvvm.UI.Forms directly to the mobile application for iOS, Android or UWP***

Call the `MvvmManager.Init` method in the shared code, such as `Application.OnStart`

    using System.Mvvm;
    ... 
    public partial class App : Application
    {
        protected override OnStart()
        {
            base.OnStart();

            MvvmManager.Init();
        }
    }

## Dependency Injection

Instead of using the static `UI` class you can use dependency injection to access the platform implementation of `IPlatformCoreUIProvider` using extensions for `IServiceCollection` provided by the platform specific packages.

After adding `DSoft.System.Mvvm.UI.Forms` to the application project you can register the core UI implementations of  `IPlatformCoreUIProvider` with the ServiceProvider during configuration of the services using the `AddCoreUI` extension method.

    using System.Mvvm;
    ... 
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            ServiceHost.Host = new HostBuilder()
             .ConfigureHostConfiguration(c =>
             {
                 c.AddCommandLine(new string[] { $"ContentRoot={FileSystem.AppDataDirectory}" });
             })
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

## IMAUIPlatformUIProvider

`IMAUIPlatformUIProvider` is a platform specific interface that the platform implementation of `` implements.  It provides access to some additonal Xamarin.Forms properties such as the current `Application`.

Use can retrieve the implementation directly from the services provider or `ServiceHost` if you are using [DSoft.System.Mvvm.Hosting](https://www.nuget.org/packages/DSoft.System.Mvvm.Hosting)

    using System.Mvvm;
    ... 

    var _formsUIProvider = ServiceHost.GetRequiredService<IMAUIPlatformUIProvider>();

    var currentApp = _formsUIProvider.CurrentApplication;