using Microsoft.Extensions.Options;
using PokeApp.Api.Options;
using System.Collections.Generic;

namespace PokeApp.Api.Validation
{
    public class ConsumerValidator : IConsumerValidator
    {
        private readonly IDictionary<string, string> consumers;

        public ConsumerValidator(IOptions<ConsumerOptions> consumers)
        {
            this.consumers = consumers.Value;
        }
        public bool Verify(string appId, string secret)
        {
            return (consumers.ContainsKey(appId) && consumers[appId] == secret);
        }
    }
}
