using LibrIO.Methode;
using System.ComponentModel.DataAnnotations;
namespace LibrIO.Classes_DTO
{
    public class MembreDTO
    {
        [Required]
        [RegularExpression(@"^[A-Z][a-zA-Zàâéèêëîïôûùüÿç-]{1,49}$")]
        [StringLength(30, MinimumLength = 4)]
        public string Nom { get; set; }
        [Required]
        [RegularExpression(@"^[A-Z][a-zA-Zàâéèêëîïôûùüÿç-]{1,49}$")]
        [StringLength(30, MinimumLength = 4)]
        public string Prenom { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
        [StringLength(64, MinimumLength = 8)]
        [EmailAddress]
        public string? Mail { get; set; }
    }
}
