﻿namespace LibrIO.Classes
{
    public class Categorie
    {
        public int? Id { get; set; }
        public string? Nom { get; set; }
        public ICollection<Livre> Livre { get; set; }
    }
}
