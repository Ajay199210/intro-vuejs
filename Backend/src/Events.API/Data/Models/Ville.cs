using Events.API.Data.Enums;

namespace Events.API.Data.Models
{
    public class Ville : BaseEntity
    {
        public string? Nom { get; set; }
        public Region? Region { get; set; }
        public int NbEvenements { get; set; }
    }
}
