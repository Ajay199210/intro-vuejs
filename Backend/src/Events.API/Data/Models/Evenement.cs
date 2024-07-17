namespace Events.API.Data.Models
{
    public class Evenement : BaseEntity
    {
        public int VilleId { get; set; }
        public List<int> CategoriesIds { get; set; }
        //public List<int> ParticipationsIds { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public string? Titre { get; set; }
        public string? Description { get; set; }
        public string? Adresse { get; set; }
        public string? NomOrganisateur { get; set; }
        public decimal PrixBillet { get; set; }
        public decimal TotaleVentes { get; set; }


        public Ville Ville { get; set; }
        public ICollection<Categorie> Categories { get; set; }
        public ICollection<Participation> Participations { get; set; }
    }
}
