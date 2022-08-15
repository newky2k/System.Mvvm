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

In the MAUI project that contains the `App` class(or other sub-class of `Application`), add  the `DSoft.System.Mvvm.UI.MAUI` package.

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

After adding `DSoft.System.Mvvm.UI.MAUI` to the application project you can register the core UI implementations of  `IPlatformCoreUIProvider` with the ServiceProvider during configuration of the services using the `AddCoreUI` extension method.

    using System.Mvvm;
    ... 
    public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				});

            builder.Services.AddCore();

			return builder.Build();
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

### ISevicesCollection extensions

On other platforms, when configuring the services there is normally a `ConfigureServices` extension to make it easier to parse a delegate to configure the services.  With `MauiAppBuilder` this is not available so we created `MauiExtensions` to add this functionality back in.

		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				})
				.ConfigureServices(services =>
                {
                    services.AddCoreUI();
                });

			return builder.Build();
		}

### MauiHostProxy

`MauiApp` does not implement `IHost` and so it cannot be used directly with [DSoft.System.Mvvm.Hosting](https://www.nuget.org/packages/DSoft.System.Mvvm.Hosting).  This makes it harder to access the `IServiceProvider` instance to retrieve the services.
We've created `MauiHostProxy` to allow you to use `MauiApp` with [DSoft.System.Mvvm.Hosting](https://www.nuget.org/packages/DSoft.System.Mvvm.Hosting) by providing a simple wrapper around it which does implement `IHost`.

You simply instatiate a new instance of `MauiHostProxy` with the output of `MauiAppBuilder.Build` and assigned it to `ServiceHost.Host` as usual.

        public static MauiApp CreateMauiApp()
        {
          var builder = MauiApp.CreateBuilder();
          builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
              fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
              fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .ConfigureServices(ConfigureServices);

          var app = builder.Build();

          //configure the servicehost host
          ServiceHost.Host = new MauiHostProxy(app);

          return app;
        }

You can then retrieve the implementation directly from `ServiceHost` as before, theres not need to keep a refernce to `MauiApp` in your shared code.

    using System.Mvvm;
    ... 

    var _coreUIProvider = ServiceHost.GetRequiredService<IPlatformCoreUIProvider>();

    await _coreUIProvider.ShowAlertAsync("Congrats", "You called the ICoreUIProvider instance");

## IMAUIPlatformUIProvider

`IMAUIPlatformUIProvider` is a platform specific interface that the platform implementation of `IPlatformCoreUIProvider` implements.  It provides access to some additonal MAUI properties such as the current `Application`.

Use can retrieve the implementation directly from the services provider or `ServiceHost` if you are using [DSoft.System.Mvvm.Hosting](https://www.nuget.org/packages/DSoft.System.Mvvm.Hosting)

    using System.Mvvm;
    ... 

    var _formsUIProvider = ServiceHost.GetRequiredService<IMAUIPlatformUIProvider>();

    var currentApp = _formsUIProvider.CurrentApplication;