using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Mvvm
{
	public class MauiHostProxy : IHost
	{
		private readonly MauiApp _app;

		public IServiceProvider Services => _app.Services;



		public MauiHostProxy(MauiApp app)
		{
			_app =	app; 
		}
		public void Dispose()
		{
			
		}

		public async Task StartAsync(CancellationToken cancellationToken = default)
		{
			await Task.Delay(1);
		}

		public async Task StopAsync(CancellationToken cancellationToken = default)
		{
			await Task.Delay(1);
		}
	}
}
