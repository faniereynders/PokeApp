namespace PokeApp.Api
{
    public interface IConsumerValidator
    {
        bool Verify(string appId, string secret);
    }
}
