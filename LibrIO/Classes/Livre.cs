namespace LibrIO.Classes
{
    public class Livre
    {
        public int? Id { get; set; }
        //public int? AuteurId { get; set; }
        public string? Titre { get; set; }
        public string? ISBN { get; set; }
        public string? Edition { get; set; }
        public bool? StatutEmprunt { get; set; }

        // clé étrangere 
        public int? AuteurId { get; set; }
        public int? CategorieId { get; set; }
        public int? GenreId { get; set; }

        // Nav 
        public Auteur? Auteur { get; set; }
        public Categorie? Categorie { get; set; }
        public Genre? Genre { get; set; }
        public ICollection<Emprunt>? Emprunt { get; set; }

    }
}
