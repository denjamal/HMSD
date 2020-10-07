using System.IO;
using Domain.Services;
using Infrastructure.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Configuration
{
    public static class ConfigurationExtension
    {
        public static void ConfigureCipher(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDataProtection()
                .SetApplicationName("HMSD.WEB.API")
                .PersistKeysToFileSystem(new DirectoryInfo("./keys"));

            serviceCollection.AddTransient<ICipher, Cipher>();
            serviceCollection.AddTransient<IKeyService, KeyService>();
        }
    }
}
