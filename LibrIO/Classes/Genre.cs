using System.Text.Json.Serialization;

namespace LibrIO.Classes
{
    public class Genre
    {
        public int Id { get; set; }
        public string? Nom { get; set; }
        [JsonIgnore]
        public ICollection<Livre>? Livres { get; set; }


    }
}
