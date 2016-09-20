using System;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace PokeApp.Api
{
    public class JwtAuthenticationOptions
    {
        public TokenValidationParameters Parameters { get; set; } = new TokenValidationParameters();
        public string TokenEndpoint { get; set; }
    }

    public class ConfigureJwtAuthenticationOptions : IConfigureOptions<JwtAuthenticationOptions>
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        public ConfigureJwtAuthenticationOptions(IServiceScopeFactory serviceScopeFactory)
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
