using Microsoft.IdentityModel.Tokens;

namespace PokeApp.Api.Options
{
    public class JwtAuthenticationOptions
    {
        public TokenValidationParameters Parameters { get; set; } = new TokenValidationParameters();
        public string TokenEndpoint { get; set; }
    }
}
