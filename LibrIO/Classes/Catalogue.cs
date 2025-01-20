namespace LibrIO.Classes
{
    public class Catalogue
    {
        public int Id { get; set; }
        public int livreId { get; set; }
        public Livre Livre { get; set; }
    }
}
