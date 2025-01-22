namespace LibrIO.Classes_DTO
{
    public class EmpruntDTO
    {
        public int Id_Livre { get; set; }
        public int Id_Membre { get; set; }
    }

    public class EmpruntDTOupdate
    {
        public int Id_Livre { get; set; }
        public int Id_Membre { get; set; }
        public DateTime DateRetour { get; set; }
    }

    public class EmpruntDTOrecherche
    {
        public int Id_Livre { get; set; }
        public int Id_Membre { get; set; }
        public DateTime DateRetour { get; set; }
    }

}