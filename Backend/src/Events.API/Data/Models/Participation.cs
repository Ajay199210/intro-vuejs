using System.ComponentModel.DataAnnotations;

namespace Events.API.Data.Models
{
    public class Participation : BaseEntity
    {
        public int EvenementId { get; set; }
        public string? NomParticipant { get; set; }
        public string? PrenomParticipant { get; set; }
        public string? AdresseCourriel { get; set; }
        public int NbPlaces { get; set; }
        public bool EstConfirmee { get; set; }


        public Evenement Evenement { get; set; }
    }
}
