using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Authentication.SqlStore.Configuration
{
    public static class ConfigHelper
    {
        public static IConfiguration GetConfig()
        {
            var builder = new ConfigurationBuilder().SetBasePath(System.AppContext.BaseDirectory)
                .AddJsonFile("sqlStoreAppSettings.json", optional: true, reloadOnChange: true);

            var path = Path.Combine(Directory.GetCurrentDirectory(), "sqlStoreAppsettings.json");

            return builder.Build();
        }
    }
}
