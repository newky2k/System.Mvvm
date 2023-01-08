using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Maui.Hosting
{
	/// <summary>
	/// Extensions for MauiAppBuilder
	/// </summary>
	public static class MauiExtensions
	{
		/// <summary>
		/// Configures the services.
		/// </summary>
		/// <param name="builder">The builder.</param>
		/// <param name="configureDelegate">The configure delegate.</param>
		/// <returns>MauiAppBuilder.</returns>
		public static MauiAppBuilder ConfigureServices(this MauiAppBuilder builder, Action<IServiceCollection> configureDelegate)
		{
			configureDelegate.Invoke(builder.Services);

			return builder;
		}
	}
}
