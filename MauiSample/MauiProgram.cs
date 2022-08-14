﻿using Microsoft.Extensions.Hosting;
using System.Mvvm;

namespace MauiSample
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				})
				.ConfigureServices(ConfigureServices);



			var app = builder.Build();

			ServiceHost.Host = new MauiHostProxy(app);

			return app;
		}

		public static void ConfigureServices(IServiceCollection services)
		{
			services.AddCoreUI();
		}
     }
}