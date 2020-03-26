using System.IO;
using Microsoft.Extensions.Configuration;

namespace Authentication.EventStore.Configuration
{
    public static class ConfigHelper
    {
        public static IConfiguration GetConfig()
        {
            var builder = new ConfigurationBuilder().SetBasePath(System.AppContext.BaseDirectory)
                .AddJsonFile("eventStoreSettings.json", optional: true, reloadOnChange: true);

            return builder.Build();
        }
    }
}
