using System.Collections.Generic;

namespace PokeApp.Api
{
    public class ClientValidator : IClientValidator
    {
        private IDictionary<string, string> applicationRegistrations = new Dictionary<string, string>()
        {
            { "app123", "appsecret123" }
        };
        public bool Verify(string appId, string secret)
        {
            return (applicationRegistrations.ContainsKey(appId) && applicationRegistrations[appId] == secret);
        }
    }
}
