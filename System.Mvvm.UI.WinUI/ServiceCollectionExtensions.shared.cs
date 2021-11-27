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
            services.TryAddSingleton<IPlatformCoreUIProvider>(PlatformUIProvider.Instance);

#if UAP
            services.TryAddSingleton<IUWPPlatformUIProvider>(PlatformUIProvider.Instance);
#endif

#if WINUI
            services.TryAddSingleton<IUWPPlatformUIProvider>(PlatformUIProvider.Instance);
#endif
            return services;
        }
    }
}
