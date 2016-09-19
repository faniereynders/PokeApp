using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokeApp.Api
{
    public interface IClientValidator
    {
        string LookupSecret(string appId);
        bool Verify(string appId, string secret);

        IEnumerable<SecurityKey> ResolveKeys(string token, SecurityToken securityToken, string kid, TokenValidationParameters validationParameters);
    }
}
