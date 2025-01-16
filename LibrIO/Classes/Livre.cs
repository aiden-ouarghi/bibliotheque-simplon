namespace LibrIO.Classes
{
    public class Livre
    {
        public int Id { get; set; }
        public string Auteur { get; set; }
        public string Titre { get; set; }
        public string ISBN { get; set; }    
        public string Edition { get; set; }
        public bool StatutEmprunt { get; set; }

    }
}
