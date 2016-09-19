using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PokeApp.Api;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static class JwtAppBuilderExtensions
    {
        private const string defaultIssuer = "http://api.pokeapp.io";
        public static IApplicationBuilder UseJwtAuthentication(this IApplicationBuilder app)
        {

            var validator = (IClientValidator)app.ApplicationServices.GetService(typeof(IClientValidator));

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidIssuers = new[] { defaultIssuer },
                IssuerSigningKeyResolver = validator.ResolveKeys
            };

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                TokenValidationParameters = tokenValidationParameters
            });
            
            
            app.MapWhen(context =>
            {
                return context.Request.Path == "/jwt/token" && 
                context.Request.Method == "POST" &&
                context.Request.Headers.Any(h=>h.Key == "Content-Type" && h.Value == "application/x-www-form-urlencoded;charset=UTF-8")
                ;
            }, config =>
            {
                config.Run(async context =>
                {

                    var key = context.Request.Headers["Authorization"][0].Split(' ')[1];

                    var credentials = getCredentials(key);

                    var valid = validator.Verify(credentials.Key, credentials.Value);
                    if (valid)
                    {
                        var accessToken = await GenerateToken(credentials.Key, credentials.Value);
                        var result = new
                        {
                            token_type = "Bearer",
                            access_token = accessToken
                        };
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
                    }
                    else
                    {

                        context.Response.StatusCode = 403;
                        await context.Response.WriteAsync("Unable to verify client");
                    }
                });
            });

            app.Use(async (context, next) =>
            {
                if (context.User.Identity.IsAuthenticated)
                {
                    await next();
                }
                else
                {

                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Invalid token");
                }

            });

            return app;
        }

        private static  KeyValuePair<string, string> getCredentials(string key)
        {
            var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(key)).Split(':');
            return new KeyValuePair<string, string>(decoded[0], decoded[1]);
        }

        private static async Task<string> GenerateToken(string appId, string secret)
        {
            var now = DateTime.UtcNow;
            var claims = new List<Claim>()
            {
                new Claim("appid", appId),

                new Claim(JwtRegisteredClaimNames.Jti, await NonceGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixDate(now).ToString(), ClaimValueTypes.Integer64)
            };

            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),

                Issuer = defaultIssuer,
                Audience = appId,

                Expires = now.Add(TimeSpan.FromMinutes(5)),



                SigningCredentials = new SigningCredentials(

    new SymmetricSecurityKey(Utilities.Base64UrlDecode(secret)), SecurityAlgorithms.HmacSha256)
            };
            var handler = new JwtSecurityTokenHandler();

            var token = handler.CreateJwtSecurityToken(tokenDescriptor);
            var encodedJwt = handler.WriteToken(token);


            return encodedJwt;
        }
        public static  Func<Task<string>> NonceGenerator { get; set; }
            = new Func<Task<string>>(() => Task.FromResult(Guid.NewGuid().ToString()));
        
        public static long ToUnixDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        static string Base64UrlEncode(byte[] arg)
        {
            string s = Convert.ToBase64String(arg); // Regular base64 encoder
            s = s.Split('=')[0]; // Remove any trailing '='s
            s = s.Replace('+', '-'); // 62nd char of encoding
            s = s.Replace('/', '_'); // 63rd char of encoding
            return s;
        }
        
    }

    
}
