using System.ComponentModel.DataAnnotations;

namespace LibrIO.Classes
{
    public class Employe
    {
        public int? Id { get; set; }
        [Required]
        [RegularExpression(@"^[A-Z][a-zA-Zàâéèêëîïôûùüÿç-]{1,49}$")]
        [StringLength(30, MinimumLength = 4)]
        public string? Nom { get; set; }

        [Required(ErrorMessage = "Le Prenom est requis pour compléter l'enregistrement.")]
        [RegularExpression(@"^[A-Z][a-zA-Zàâéèêëîïôûùüÿç-]{1,49}$",
       ErrorMessage = "Le prénom n'est pas valide. Il doit commencer par une majuscule et ne doit contenir que des lettres.")]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "Le prénom doit contenir entre 4 et 30 caractères.")]
        public string? Prenom { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
        [StringLength(64, MinimumLength =  8)]
        [EmailAddress]
        public string? Mail { get; set; }

        public string? Role { get; set; }

    }
}
