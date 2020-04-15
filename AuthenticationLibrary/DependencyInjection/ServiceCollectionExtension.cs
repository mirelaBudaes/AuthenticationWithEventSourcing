using Authentication.Command;
using Authentication.Command.DependencyInjection;
using Authentication.Command.Handlers;
using Authentication.Command.Sagas;
using Authentication.EventStore;
using Authentication.Infrastructure;
using Authentication.Query.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication.Library.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddAuthenticationLibrary(this IServiceCollection services)
        {
            services.AddCommandLibrary();
            services.AddQueryLibrary();
            services.AddTransient<IAuthenticationService, AuthenticationService>();

            services.AddSingleton<IBus>(s =>
            {
                var eventRepo = s.GetService<IAuthenticationEventRepository>();
                var userSqlRepo = s.GetService<UserSyncronizer>();
                var bus = new InMemoryBus(eventRepo, userSqlRepo);
                bus.RegisterHandler<RegisterUserHandler>();
                bus.RegisterSaga<RegisterSaga>();

                return bus;
            });
            
            return services;
        }
    }
}
