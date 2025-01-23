using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LibrIO.Classes
{
    public class Categorie
    {
        public int? Id { get; set; }
       
        public string? Nom { get; set; }

        [JsonIgnore]
        public ICollection<Livre>? Livre { get; set; }

    }


}
