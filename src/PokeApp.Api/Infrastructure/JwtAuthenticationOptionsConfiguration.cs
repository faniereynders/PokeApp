using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PokeApp.Api.Options;
using System;

namespace PokeApp.Api.Infrastructure
{
    public class JwtAuthenticationOptionsConfiguration : IConfigureOptions<JwtAuthenticationOptions>
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        public JwtAuthenticationOptionsConfiguration(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public void Configure(JwtAuthenticationOptions options)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var provider = scope.ServiceProvider;
                var configuration = provider.GetRequiredService<IConfigurationRoot>();
                var jwtConfigs = configuration.GetSection(nameof(JwtAuthenticationOptions));

                if (jwtConfigs[nameof(TokenValidationParameters.IssuerSigningKey)] == null)
                {
                    throw new ArgumentNullException("IssuerSigningKey not found. Add key value to configuration.");
                }
                options.Parameters = new TokenValidationParameters
                {
                    ValidIssuer = jwtConfigs[nameof(TokenValidationParameters.ValidIssuer)],
                    ValidateAudience = Convert.ToBoolean((jwtConfigs[nameof(TokenValidationParameters.ValidateAudience)])),
                    IssuerSigningKey = new SymmetricSecurityKey(Utilities.Base64UrlDecode(jwtConfigs[nameof(TokenValidationParameters.IssuerSigningKey)]))
                };
                options.TokenEndpoint = jwtConfigs[nameof(JwtAuthenticationOptions.TokenEndpoint)];
            }
        }
    }
}
