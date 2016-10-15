namespace PokeApp.Api.Models
{
    public class Pokemon
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string ImageUrl
        {
            get
            {
                return $"https://img.pokemondb.net/artwork/{Name.ToLower()}.jpg";
            }
        }
    }
}
