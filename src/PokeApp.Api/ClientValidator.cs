using System;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace PokeApp.Api
{
    public class ClientValidator : IClientValidator
    {
        private IDictionary<string, string> applicationRegistrations = new Dictionary<string, string>()
        {
            { "app123", "superdupersecretkey123" }
        };

        public string LookupSecret(string appId)
        {
            return applicationRegistrations[appId];
        }

        public IEnumerable<SecurityKey> ResolveKeys(string token, SecurityToken securityToken, string kid, TokenValidationParameters validationParameters)
        {
            var jwtToken = securityToken as JwtSecurityToken;
            var keys = new List<SecurityKey>();

            var appId = jwtToken.Claims.SingleOrDefault(c => c.Type == "appid")?.Value;
            if (appId != null && applicationRegistrations.ContainsKey(appId))
            {
                keys.Add(new SymmetricSecurityKey(Utilities.Base64UrlDecode(applicationRegistrations[appId])));
            }
           
            return keys;
        }

        public bool Verify(string appId, string secret)
        {
            if (applicationRegistrations.ContainsKey(appId))
            {


                var appSecret = applicationRegistrations[appId];

                return (appSecret == secret);
            }
            return false;
        }
    }
}
