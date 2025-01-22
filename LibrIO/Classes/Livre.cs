using LibrIO.Classes_DTO;
using System.Text.Json.Serialization;


namespace LibrIO.Classes
{
    public class Livre
    {
        public int? Id { get; set; }
        public string? Titre { get; set; }
        public string? ISBN { get; set; }
        public string? Edition { get; set; }
        public bool? StatutEmprunt { get; set; }
        public bool? Disponibilite { get; set; }

        // clé étrangere 
        public int? AuteurId { get; set; }
        public int? CategorieId { get; set; }
        public int? GenreId { get; set; }

        // Nav 
        [JsonIgnore]
        public Auteur Auteur { get; set; }
        [JsonIgnore]
        public Categorie? Categorie { get; set; }
        [JsonIgnore]
        public Genre? Genre { get; set; }
        [JsonIgnore] // Empêche la sérialisation de cette propriété
        public ICollection<Emprunt>? Emprunt { get; set; }

    }
}
