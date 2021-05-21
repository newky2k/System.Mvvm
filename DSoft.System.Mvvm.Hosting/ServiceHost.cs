using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Mvvm
{
    public static class ServiceHost
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
    }
}
