﻿using Authentication.EventStore.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication.EventStore.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddEventStoreLibrary(this IServiceCollection services)
        {
            services.AddTransient<IAuthenticationEventRepository, AuthenticationEventRepository>();

            services.AddSingleton<DatabaseConnectionStrings>();
            //services.AddSingleton<MemoryEventDb>();
            services.AddTransient<IEventStore, LiteDbStore>();

            return services;
        }
    }
}
