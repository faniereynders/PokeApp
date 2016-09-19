using System.Collections.Generic;

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

        public bool Verify(string appId, string secret)
        {
            var appSecret = applicationRegistrations[appId];

            return (appSecret == secret);
        }
    }
}
