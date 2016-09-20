namespace PokeApp.Api
{
    public interface IClientValidator
    {
        bool Verify(string appId, string secret);
    }
}
