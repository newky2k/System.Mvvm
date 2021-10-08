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
        #region Fields

        private static IHost _host;
        private static IHost _secondaryHost;

        #endregion

        #region Properties

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

        #endregion


        #region Methods



        /// <summary>
        /// Gets the required service with an existing scope
        /// </summary>
        /// <typeparam name="T">Service Type</typeparam>
        /// <param name="scope">The current scope.</param>
        /// <returns></returns>
        public static T GetRequiredService<T>(IServiceScope scope) => scope.ServiceProvider.GetRequiredService<T>();

        /// <summary>
        /// Gets the specified service with a new scope, will return null if not found
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetService<T>() => Host.Services.CreateScope().ServiceProvider.GetService<T>();

        /// <summary>
        /// Gets the required service with a new scope, will throw exception if not found
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetRequiredService<T>() => Host.Services.CreateScope().ServiceProvider.GetRequiredService<T>();


        /// <summary>
        /// Gets the specified service with a new scope from the secondary host, will return null if not found
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetSecondaryService<T>() => SecondaryHost.Services.CreateScope().ServiceProvider.GetService<T>();

        /// <summary>
        /// Gets the required service with a new scope, will throw exception if not found
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetRequiredSecondaryService<T>() => SecondaryHost.Services.CreateScope().ServiceProvider.GetRequiredService<T>();


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

        /// <summary>
        /// Creates a new scope from the primary Host
        /// </summary>
        /// <returns></returns>
        public static IServiceScope CreateScope() => Host.Services.CreateScope();

        /// <summary>
        /// Creates a new scope from the secondary Host.
        /// </summary>
        /// <returns></returns>
        public static IServiceScope CreateSecondaryScope() => SecondaryHost.Services.CreateScope();

        #endregion
    }
}
