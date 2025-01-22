﻿using System.Text.Json.Serialization;

namespace LibrIO.Classes
{
    public class Membre
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Mail { get; set; }
        [JsonIgnore]
        public ICollection<Emprunt> Emprunts { get; set; }

    }
}
