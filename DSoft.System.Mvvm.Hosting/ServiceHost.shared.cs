using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Mvvm
{
    public static partial class ServiceHost
    {
        private static IHost _host;

        public static IHost Host
        {
            get 
            {
                if (_host == null)
                    throw new NullReferenceException("The Host property has not been set on ServiceHost");


                return _host; 
            }
            set { _host = value; }
        }

        public static IServiceProvider Provider => Host.Services;

        public static IServiceScope Scope => Provider.CreateScope();

        /// <summary>
        /// Gets the required service with an existing scope
        /// </summary>
        /// <typeparam name="T">Service Type</typeparam>
        /// <param name="scope">The current scope.</param>
        /// <returns></returns>
        public static T GetRequiredService<T>(IServiceScope scope) => scope.ServiceProvider.GetRequiredService<T>();

        /// <summary>
        /// Gets the required service with a new scope
        /// </summary>
        /// <typeparam name="T">Service Type</typeparam>
        /// <returns></returns>
        public static T GetRequiredService<T>() => Provider.CreateScope().ServiceProvider.GetRequiredService<T>();

        /// <summary>
        /// Starts the host asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public static Task StartAsync(CancellationToken cancellationToken = default) => Host.StartAsync();

        /// <summary>
        /// Stops the host asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public static Task StopAsync(CancellationToken cancellationToken = default) => Host.StopAsync();

        /// <summary>
        /// Initializes the ServiceHost
        /// </summary>
        /// <param name="hostConfigurationBuilder">The host configuration builder.</param>
        /// <param name="serviceConfigurationAction">The service configuration action.</param>
        public static void Init(Action<IConfigurationBuilder> hostConfigurationBuilder, Action<HostBuilderContext, IServiceCollection> servicesConfigurationAction)
        {

            Init((builder) =>
            {
                if (hostConfigurationBuilder != null)
                    builder = builder.ConfigureHostConfiguration(hostConfigurationBuilder);

                if (servicesConfigurationAction != null)
                    builder = builder.ConfigureServices(servicesConfigurationAction);
            });

        }

        /// <summary>
        /// Initializes the ServiceHost with a specified root path.
        /// </summary>
        /// <param name="rootPath">The root path.</param>
        /// <param name="serviceConfigurationAction">The service configuration action.</param>
        public static void Init(string rootPath, Action<HostBuilderContext, IServiceCollection> servicesConfigurationAction) => Init((c) => { c.AddCommandLine(new string[] { $"ContentRoot={rootPath}" }); }, servicesConfigurationAction);

        /// <summary>
        /// Initializes the ServiceHost
        /// </summary>
        /// <param name="serviceConfigurationAction">The service configuration action.</param>
        public static void Init(Action<HostBuilderContext, IServiceCollection> servicesConfigurationAction) => Init((c) => { }, servicesConfigurationAction);

        /// <summary>
        /// Initializes the ServiceHost with the specified configure services action.
        /// </summary>
        /// <param name="configureServicesAction">The configure services action.</param>
        public static void Init(Action<IConfiguration, IServiceCollection> configureServicesAction)
        {
            Init((HostBuilderContext context, IServiceCollection services) =>
            {
                configureServicesAction?.Invoke(context.Configuration, services);

            });
        }

        /// <summary>
        /// Initializes the specified root path.
        /// </summary>
        /// <param name="rootPath">The root path.</param>
        /// <param name="configureServicesAction">The configure services action.</param>
        public static void Init(string rootPath, Action<IConfiguration, IServiceCollection> configureServicesAction)
        {
            Init(rootPath, (HostBuilderContext context, IServiceCollection services) =>
            {
                configureServicesAction?.Invoke(context.Configuration, services);

            });
        }

        /// <summary>
        /// Initializes a new ServiceHost instance the specified configuration action.
        /// </summary>
        /// <param name="configAction">The configuration action.</param>
        public static void Init(Action<IHostBuilder> configAction)
        {
            var host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder();

            configAction?.Invoke(host);

            Host = host.Build();


        }
    }
}
