using Events.API.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace Events.API.DTO
{
    // Pour l'affichage
    public class VilleDTO
    {
        public int Id { get; set; }
        public string? Nom { get; set; }
        public Region? Region { get; set; }
    }

    // Pour l'ajout et la modification d'une ville
    public class VillePostPutDTO
    {
        [Required(ErrorMessage = "La nom de la ville est obligatoire")]
        public string? Nom { get; set; }

        //[Required(ErrorMessage = "La région de la ville est obligatoire")]
        public Region? Region { get; set; }
    }

    // Pour le controlleur statistique
    public class VilleStatsDTO
    {
        public int Id { get; set; }
        public string? Nom { get; set; }
        public Region? Region { get; set; }
        public int NbEvenements { get; set; }
    }
}
