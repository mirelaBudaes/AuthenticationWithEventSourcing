using Authentication.EventStore.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication.Command.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCommandLibrary(this IServiceCollection services)
        {
            services.AddEventStoreLibrary();
            services.AddSingleton<IEventSourceManager, EventSourceManager>();
            services.AddTransient<UserSqlRepository>();
            services.AddTransient<UserSyncronizer>();
            return services;
        }
    }
}
