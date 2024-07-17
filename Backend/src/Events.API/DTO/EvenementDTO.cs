using System.ComponentModel.DataAnnotations;

namespace Events.API.DTO
{
    // Pour l'affichage d'un événement
    public class EvenementDTO
    {
        public int Id { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public string? Titre { get; set; }
        public string? Description { get; set; }
        public string? Adresse { get; set; }
        public string? NomOrganisateur { get; set; }
        public decimal PrixBillet { get; set; }
        public decimal TotaleVentes { get; set; }


        public int VilleId { get; set; }
        public List<int> CategoriesIds { get; set; }
        public List<int> ParticipationsIds { get; set; }
    }


    // Pour l'ajout et la modification d'un événement puisqu'on a pas besoin de la liste des participations
    public class EvenementPostPutDTO
    {
        [Required(ErrorMessage = "La date de début est obligatoire")]
        public DateTime DateDebut { get; set; }

        [Required(ErrorMessage = "La date de fin est obligatoire")]
        public DateTime DateFin { get; set; }

        [Required(ErrorMessage = "Le titre de l'événement est obligatoire")]
        public string? Titre { get; set; }

        [Required(ErrorMessage = "La description de l'événement est obligatoire")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "L'adresse de l'événement est obligatoire")]
        public string? Adresse { get; set; }

        [Required(ErrorMessage = "Le nom de l'organisateur est obligatoire")]
        public string? NomOrganisateur { get; set; }

        public decimal PrixBillet { get; set; }


        public int VilleId { get; set; }
        public List<int> CategoriesIds { get; set; }
    }
}
