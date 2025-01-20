namespace LibrIO.Classes
{
    public class Auteur
    {
        public int? Id { get; set; }
        public string? Nom { get; set; }
        public string? Prenom { get; set; }
        public ICollection<Livre> Livre { get; set; }
    }
}
