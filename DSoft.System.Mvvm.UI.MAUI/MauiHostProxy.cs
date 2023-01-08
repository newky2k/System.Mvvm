using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Mvvm
{
	/// <summary>
	/// Proxy service host for MAUI
	/// Implements the <see cref="IHost" />
	/// </summary>
	/// <seealso cref="IHost" />
	public class MauiHostProxy : IHost
	{
		private readonly MauiApp _app;

		/// <summary>
		/// The services from the MauiApp
		/// </summary>
		/// <value>The services.</value>
		public IServiceProvider Services => _app.Services;



		/// <summary>
		/// Initializes a new instance of the <see cref="MauiHostProxy"/> class.
		/// </summary>
		/// <param name="app">The application.</param>
		public MauiHostProxy(MauiApp app)
		{
			_app =	app; 
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			
		}

		/// <summary>
		/// Empty implementation of the Start method to fufill the interface
		/// </summary>
		/// <param name="cancellationToken">Used to abort program start.</param>
		/// <returns>A Task representing the asynchronous operation.</returns>
		public Task StartAsync(CancellationToken cancellationToken = default) => Task.Delay(1);

		/// <summary>
		/// Empty implementation of the Stop method to fufill the interface
		/// </summary>
		/// <param name="cancellationToken">Used to indicate when stop should no longer be graceful.</param>
		/// <returns>A Task representing the asynchronous operation.</returns>
		public Task StopAsync(CancellationToken cancellationToken = default) => Task.Delay(1);
	}
}
