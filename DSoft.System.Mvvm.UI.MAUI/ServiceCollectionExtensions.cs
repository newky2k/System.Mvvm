using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Mvvm;
using System.Text;
using System.Threading.Tasks;


namespace Microsoft.Extensions.DependencyInjection
{
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		/// Adds the core MAUI UI providers to the available services
		/// </summary>
		/// <param name="services">The services.</param>
		/// <returns></returns>
		public static IServiceCollection AddCoreUI(this IServiceCollection services)
		{
			services.TryAddSingleton<IPlatformCoreUIProvider>(PlatformUIProvider.Instance);

			services.TryAddSingleton<IMAUIPlatformUIProvider>(PlatformUIProvider.Instance);

			return services;
		}
	}
}

