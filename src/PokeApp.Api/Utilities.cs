using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PokeApp.Api
{
    public static class Utilities
    {
        public static byte[] Base64UrlDecode(string arg)
        {
            string s = arg;
            s = s.Replace('-', '+'); // 62nd char of encoding
            s = s.Replace('_', '/'); // 63rd char of encoding
            switch (s.Length % 4) // Pad with trailing '='s
            {
                case 0: break; // No pad chars in this case
                case 2: s += "=="; break; // Two pad chars
                case 3: s += "="; break; // One pad char
                default:
                    throw new System.Exception(
             "Illegal base64url string!");
            }
            return Convert.FromBase64String(s); // Standard base64 decoder
        }

        public static KeyValuePair<string, string> GetCredentialsFromAuthorizationHeader(string headerValue)
        {
            var key = headerValue.Split(' ')[1];
            var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(key)).Split(':');
            return new KeyValuePair<string, string>(decoded[0], decoded[1]);
        }
        public static Func<Task<string>> NonceGenerator { get; set; }
          = new Func<Task<string>>(() => Task.FromResult(Guid.NewGuid().ToString()));

        public static long ToUnixDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        public static async Task<string> GenerateToken(string appId, SecurityKey key, string issuer, TimeSpan lifetime)
        {
            var now = DateTime.UtcNow;
            var claims = new List<Claim>()
            {
                new Claim("appid", appId),
                new Claim(JwtRegisteredClaimNames.Jti, await NonceGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixDate(now).ToString(), ClaimValueTypes.Integer64)
            };
            var identity = new ClaimsIdentity(claims);
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateJwtSecurityToken(issuer, appId, identity, now, now.Add(lifetime), now, signingCredentials);
            
            var encodedJwt = handler.WriteToken(token);
            
            return encodedJwt;
        }
    }
}
