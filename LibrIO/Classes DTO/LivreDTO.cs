namespace LibrIO.Classes_DTO
{
    public class LivreDTO
    {
        public string? Titre { get; set; }
        public string? ISBN { get; set; }
        public string? Edition { get; set; }
        public int? AuteurId { get; set; }
        public int? GenreId { get; set; }
        public int? CategorieId { get; set; }
    }
}
