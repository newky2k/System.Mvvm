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
        private static IHost _secondaryHost;

        /// <summary>
        /// Gets or sets the primary global IHost instance
        /// </summary>
        /// <value>
        /// The primary host.
        /// </value>
        /// <exception cref="System.NullReferenceException">The Host property has not been set on ServiceHost</exception>
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

        /// <summary>
        /// Gets or sets the primary global IHost instance
        /// </summary>
        /// <value>
        /// The secondary host.
        /// </value>
        /// <exception cref="System.NullReferenceException">The SecondaryHost property has not been set on ServiceHost</exception>
        public static IHost SecondaryHost
        {
            get
            {
                if (_secondaryHost == null)
                    throw new NullReferenceException($"The SecondaryHost property has not been set on ServiceHost");

                return _secondaryHost;
            }
            set { _secondaryHost = value; }
        }

        public static IServiceProvider Provider => Host.Services;

        public static IServiceProvider SecondaryProvider => SecondaryHost.Services;

        public static IServiceScope Scope => Provider.CreateScope();

        public static IServiceScope SecondaryScope => SecondaryProvider.CreateScope();

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
        /// <typeparam name="T"></typeparam>
        /// <param name="hostMode">The host to use.</param>
        /// <returns></returns>
        public static T GetRequiredService<T>(HostMode hostMode = HostMode.Primary)
        {
            if (hostMode == HostMode.Primary)
                return Provider.CreateScope().ServiceProvider.GetRequiredService<T>();
            else
                return SecondaryProvider.CreateScope().ServiceProvider.GetRequiredService<T>();
        }

        /// <summary>
        /// Starts the host asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public static Task StartAsync(HostMode hostMode = HostMode.Primary, CancellationToken cancellationToken = default)
        {
            if (hostMode == HostMode.Primary)
                return Host.StartAsync(cancellationToken);
            else
                return SecondaryHost.StartAsync(cancellationToken);
        }

        /// <summary>
        /// Stops the host asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public static Task StopAsync(HostMode hostMode = HostMode.Primary, CancellationToken cancellationToken = default)
        {
            if (hostMode == HostMode.Primary)
                return Host.StopAsync(cancellationToken);
            else
                return SecondaryHost.StopAsync(cancellationToken);
        }

        /// <summary>
        /// Initializes the ServiceHost
        /// </summary>
        /// <param name="hostConfigurationBuilder">The host configuration builder.</param>
        /// <param name="serviceConfigurationAction">The service configuration action.</param>
        public static IHost Initialize(Action<IConfigurationBuilder> hostConfigurationBuilder, Action<HostBuilderContext, IServiceCollection> servicesConfigurationAction, HostMode hostMode = HostMode.Primary)
        {

            IHostBuilder builder = new HostBuilder();

            if (hostConfigurationBuilder != null)
                builder = builder.ConfigureHostConfiguration(hostConfigurationBuilder);

            if (servicesConfigurationAction != null)
                builder = builder.ConfigureServices(servicesConfigurationAction);

            var aHost = builder.Build();

            if (hostMode == HostMode.Primary)
                Host = aHost;
            else
                SecondaryHost = aHost;

            return aHost;
        }

        /// <summary>
        /// Initializes the ServiceHost with a specified root path.
        /// </summary>
        /// <param name="rootPath">The root path.</param>
        /// <param name="serviceConfigurationAction">The service configuration action.</param>
        public static IHost Initialize(string rootPath, Action<HostBuilderContext, IServiceCollection> servicesConfigurationAction, HostMode hostMode = HostMode.Primary) => Initialize((c) => { c.AddCommandLine(new string[] { $"ContentRoot={rootPath}" }); }, servicesConfigurationAction, hostMode);

        /// <summary>
        /// Initializes the ServiceHost
        /// </summary>
        /// <param name="serviceConfigurationAction">The service configuration action.</param>
       public static IHost Initialize(Action<HostBuilderContext, IServiceCollection> configAction, HostMode hostMode = HostMode.Primary)
        {
            var aHost = new HostBuilder().ConfigureServices(configAction).Build();

            if (hostMode == HostMode.Primary)
                Host = aHost;
            else
                SecondaryHost = aHost;

            return aHost;
        }



    }
}
