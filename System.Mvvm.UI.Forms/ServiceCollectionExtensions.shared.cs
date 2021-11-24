using Microsoft.Extensions.DependencyInjection;
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
        public static IServiceCollection AddCoreUI(this IServiceCollection services)
        {
            services.TryAddTransient<IPlatformCoreUIProvider, PlatformUIProvider>();

            services.TryAddTransient<IXamarinFormsPlatformUIProvider, PlatformUIProvider>();

            return services;
        }
    }
}
