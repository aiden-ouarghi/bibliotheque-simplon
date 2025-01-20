namespace LibrIO.Classes
{
    public class Genre
    {
        public int Id { get; set; }
        public string? Nom { get; set; }
        public ICollection<Livre>? Livres { get; set; }
    }
}
