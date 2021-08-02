using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace System.Mvvm
{
    public static class ViewModelExtensions
    {
        /// <summary>
        /// Retrieves the required service from the ServiceHost
        /// </summary>
        /// <typeparam name="T">Type of service to retrieve</typeparam>
        /// <param name="viewModel">The target view model</param>
        /// <returns></returns>
        public static T GetRequiredService<T>(this ViewModel viewModel) => ServiceHost.GetRequiredService<T>();

        /// <summary>
        /// Retrieves the required service from the ServiceHost
        /// </summary>
        /// <typeparam name="T">Type of service to retrieve</typeparam>
        /// <param name="viewModel">The target view model</param>
        /// <param name="scope">An existing scope to use</param>
        /// <returns></returns>
        public static T GetRequiredService<T>(this ViewModel viewModel, IServiceScope scope) => scope.ServiceProvider.GetRequiredService<T>();
    }

}
