using BusinessAbstraction;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLibrary
{
    public static class BusinessLayerExtension
    {
        public static IServiceCollection AddBusinessLibrary(this IServiceCollection services)
        {
            services.AddScoped<IAuthRepository, AuthRepository>();

            return services;
        }
    }
}
