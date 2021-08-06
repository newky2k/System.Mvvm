using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Mvvm.Contracts;
using System.Mvvm.Ui;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCoreUI(this IServiceCollection services)
        {
            services.TryAddSingleton<IPlatformCoreUIProvider, PlatformUIProvider>();
        }
    }
}
