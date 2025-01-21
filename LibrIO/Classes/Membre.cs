using System.Text.Json.Serialization;

namespace LibrIO.Classes
{
    public class Membre
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Mail { get; set; }


        [JsonIgnore] // Empêche la sérialisation de cette propriété
        public ICollection<Emprunt> Emprunt { get; set; }

    }
}
