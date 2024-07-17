using System.ComponentModel.DataAnnotations;

namespace Events.API.DTO
{
    public class CategorieDTO
    {
        public int Id { get; set; }
        public string? Nom { get; set; }
    }

    public class CategoriePostPutDTO
    {
        [Required(ErrorMessage = "Le nom de la catégorie est obligatoire")]
        public string? Nom { get; set; }
    }
}
