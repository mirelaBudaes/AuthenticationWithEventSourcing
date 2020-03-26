using Authentication.SqlStore.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication.Query.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddQueryLibrary(this IServiceCollection services)
        {
            services.AddSqlStoreLibrary();
            services.AddTransient<UserRepository>();
            return services;
        }
    }
}
