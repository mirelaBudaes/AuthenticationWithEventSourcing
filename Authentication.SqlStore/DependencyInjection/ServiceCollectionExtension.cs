using Authentication.SqlStore.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication.SqlStore.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddSqlStoreLibrary(this IServiceCollection services)
        {
            services.AddSingleton<DatabaseConnectionStrings>();
            return services;
        }
    }
}
