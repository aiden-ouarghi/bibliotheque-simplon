using System.ComponentModel.DataAnnotations;

namespace LibrIO.Classes_DTO
{
    public class GenreDTO
    {
        [RegularExpression(@"^[a-zA-Z]+$")]
        [StringLength(30, MinimumLength = 2)]
        [Required]
        public string? Nom { get; set; }

    }
}
