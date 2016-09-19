using PokeApp.Api;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class JwtServiceCollectionExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
        {
            services
                .AddSingleton<IClientValidator, ClientValidator>()
                .AddAuthentication();

            return services;
        }
    }
}
