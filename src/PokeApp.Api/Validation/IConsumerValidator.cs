namespace PokeApp.Api.Validation
{
    public interface IConsumerValidator
    {
        bool Verify(string appId, string secret);
    }
}
