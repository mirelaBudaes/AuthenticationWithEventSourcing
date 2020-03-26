using Authentication.Command.DependencyInjection;
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
            return services;
        }
    }
}
