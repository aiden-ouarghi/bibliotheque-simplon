using LibrIO.Classes;
using System.ComponentModel.DataAnnotations;

namespace LibrIO.Classes_DTO
{
    public class AuteurDTO
    {
        [Required]
        public string? Nom { get; set; }
        [Required]
        public string? Prenom { get; set; }
    }
}
