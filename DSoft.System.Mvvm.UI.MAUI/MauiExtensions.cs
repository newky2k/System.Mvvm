using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Maui.Hosting
{
	public static class MauiExtensions
	{
		public static MauiAppBuilder ConfigureServices(this MauiAppBuilder builder, Action<IServiceCollection> configureDelegate)
		{
			configureDelegate.Invoke(builder.Services);

			return builder;
		}
	}
}
