# System.Mvvm.Hosting

Microsoft.Extensions.Hosting helper library and System.Mvvm extensions

### Functionality
- Centralised ServiceProvider Host instance
  - Works with Web, Mobile and Desktop applications
  - Simplified initialisation methods for creating and configuring the ServiceProvider host
- Extensions for `System.Mvvm.ViewModel` to make it easy to get Services from the ServiceProvider host
- Support for .Net Standard 2.0 and .NET 5.0

# ServiceHost

The `ServiceHost` class is the centralised wrapper for a global instance of `IHost` that can be used with ASP.NET core applications, Desktop and Mobile applications too.

The `Init` method and its many overloads, simplifies and standardises the initialisation and configuration of the `IHost` instance.

For example, this is the simplest way to configure, build and optionally start a instance of `IHost`

    using System.Mvvm;
    ....

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        await ServiceHost
            .Init(ConfigureServices)
            .StartAsync();
    }

     void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
     {
         services.AddCoreUI();
         services.TryAddSingleton<ITestCustomUIProvider, TestCustomUIProvider>();
     }

To access retrieve a service, use `ServiceHost.GetRequiredService<T>`. 

    using System.Mvvm;
    ...

    var customUI = ServiceHost.GetRequiredService<ITestCustomUIProvider>();
    customUI.SayHello();

You can also `ServiceHost.Host` can be used to access the `IHost` instance directly and uses the standad access methods.


