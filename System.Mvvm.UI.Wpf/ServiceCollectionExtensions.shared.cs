using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Mvvm;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
	/// <summary>
	/// IServiceCollection Extensions.
	/// </summary>
	public static class ServiceCollectionExtensions
    {
		/// <summary>
		/// Adds the core UI for WPF
		/// </summary>
		/// <param name="services">The services.</param>
		/// <returns>IServiceCollection.</returns>
		public static IServiceCollection AddCoreUI(this IServiceCollection services)
        {
            services.TryAddSingleton<IPlatformCoreUIProvider>(PlatformUIProvider.Instance);

            services.TryAddSingleton<IWPFPlatformUIProvider>(PlatformUIProvider.Instance);

            return services;
        }
    }
}
