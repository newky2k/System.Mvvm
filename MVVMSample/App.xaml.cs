using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using MVVMSample.Contracts;
using MVVMSample.Providers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Mvvm;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MVVMSample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MvvmManager.Init();

            ServiceHost.Host = new HostBuilder()
                        .ConfigureServices(ConfigureServices)
                        .Build();

            var wpfPlatform = ServiceHost.GetRequiredService<IWPFPlatformUIProvider>();

            wpfPlatform.ShowAlertOverideFunction = ShowAlertWindow;

            wpfPlatform.ShowConfirmOverideFunction = ShowConfirmationDialog;

        }

        public static Task ShowAlertWindow(string title, string message)
        {
            var wpfPlatform = ServiceHost.GetRequiredService<IWPFPlatformUIProvider>();

            var currentWindow = wpfPlatform.CurrentWindow as MetroWindow;

            return DialogManager.ShowMessageAsync(currentWindow, title, message);
        }

        public static Task<bool> ShowConfirmationDialog(string title, string message)
        {
            var tcs = new TaskCompletionSource<bool>();

            var wpfPlatform = ServiceHost.GetRequiredService<IWPFPlatformUIProvider>();

            UI.InvokeOnUIThread(async () =>
            {
                var currentWindow = wpfPlatform.CurrentWindow as MetroWindow;

                var result = await DialogManager.ShowMessageAsync(currentWindow, title, message, MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings()
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No",
                });

                tcs.SetResult((result == MessageDialogResult.Affirmative));
            });
            

            return tcs.Task;
        }

        void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
        {
            services.AddCoreUI();
            services.TryAddSingleton<ITestCustomUIProvider, TestCustomUIProvider>();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            if (ServiceHost.Host != null)
            {
                await ServiceHost.Host.StopAsync();

            }
            base.OnExit(e);
        }
    }
}
