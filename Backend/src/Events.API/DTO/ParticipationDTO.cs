using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Events.API.DTO
{
    // V1 pour confirmer (et afficher) la participation (status)
    public class ParticipationDTO
    {
        public int Id { get; set; }
        public string? NomParticipant { get; set; }
        public string? PrenomParticipant { get; set; }
        public string? AdresseCourriel { get; set; }
        public int NbPlaces { get; set; }

        [JsonIgnore]
        public bool EstConfirmee { get; set; } = false;


        public int EvenementId { get; set; }
    }

    // V2 pour l'ajout d'une participation
    public class ParticipationPostDTO
    {
        [Required(ErrorMessage = "Le nom du participant est obligatoire")]
        public string? NomParticipant { get; set; }

        [Required(ErrorMessage = "Le prénom du participant est obligatoire")]
        public string? PrenomParticipant { get; set; }

        [Required(ErrorMessage = "L'adresse courriel est obligatoire")]
        [EmailAddress(ErrorMessage = "Le format de l'adresse courriel n'est pas correcte")]
        public string? AdresseCourriel { get; set; }

        public int NbPlaces { get; set; }
        public int EvenementId { get; set; }
    }
}
