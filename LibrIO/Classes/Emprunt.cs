using System.ComponentModel.DataAnnotations;

namespace LibrIO.Classes
{
    public class Emprunt
    {
        [Key]
        public int Id { get; set; }
        public DateTime DateEmprunt { get; set; }
        public DateTime DateRetour { get; set; }

        // Foreign keys vers la table livre et la table membre 
        public int Id_Livre { get; set; }
        public int Id_Membre { get; set; }
        public bool Encours { get; set; }

        // Navigation 
        public Livre? Livre { get; set; }
        public Membre? Membre { get; set; }
    }
}
