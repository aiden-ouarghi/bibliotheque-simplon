using System.ComponentModel.DataAnnotations;

namespace LibrIO.Classes_DTO
{
    public class LivreDTO
    {
        [StringLength(150, MinimumLength = 1)]
        public string? Titre { get; set; }

        [RegularExpression(@"^(?:ISBN(?:-13)?:? )?(?:978|979)\d{10}$",
          ErrorMessage = "Format ISBN-13 invalide")]
        public string? ISBN { get; set; }

        public string? Edition { get; set; }

        [RegularExpression(@"^[1-9]")]
        public int? AuteurId { get; set; }
        [RegularExpression(@"^[1-9]")]
        public int? GenreId { get; set; }
        [RegularExpression(@"^[1-9]")]
        public int? CategorieId { get; set; }
    }
}
