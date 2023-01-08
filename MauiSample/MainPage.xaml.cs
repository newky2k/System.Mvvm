using System.Mvvm;

namespace MauiSample
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

		private async void OnCounterClicked(object sender, EventArgs e)
		{
			var coreUIServices = ServiceHost.GetRequiredService<IPlatformCoreUIProvider>();

			await coreUIServices.ShowAlertAsync("Hi", "Hello world");
			//count++;

			//if (count == 1)
			//	CounterBtn.Text = $"Clicked {count} time";
			//else
			//	CounterBtn.Text = $"Clicked {count} times";

			//SemanticScreenReader.Announce(CounterBtn.Text);
		}
	}
}