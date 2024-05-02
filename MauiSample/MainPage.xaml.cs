using System.Mvvm;

namespace MauiSample
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnCounterClicked(object sender, EventArgs e)
        {
            var coreUIServices = ServiceHost.GetRequiredService<IPlatformCoreUIProvider>();

            await coreUIServices.ShowAlertAsync("Hi", "Hello world");
        }
    }

}
