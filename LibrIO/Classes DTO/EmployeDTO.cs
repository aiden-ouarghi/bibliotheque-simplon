using System.ComponentModel.DataAnnotations;

namespace LibrIO.Classes_DTO
{
    public class EmployeDTO
    {
        [Required(ErrorMessage = "Le Nom est requis pour compléter l'enregistrement.")]
        [RegularExpression(@"^[A-Z][a-zA-Zàâéèêëîïôûùüÿç-]{1,49}$",
       ErrorMessage = "Le prénom n'est pas valide. Il doit commencer par une majuscule et ne doit contenir que des lettres.")]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "Le prénom doit contenir entre 4 et 30 caractères.")]
        public string? Nom { get; set; }
        [Required]
        [RegularExpression(@"^[A-Z][a-zA-Zàâéèêëîïôûùüÿç-]{1,49}$")]
        [StringLength(30, MinimumLength = 4)]
        public string? Prenom { get; set; }
        public string? Mail { get; set; }
        public string? Role { get; set; }

    }
}
