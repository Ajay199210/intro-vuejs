using Events.API.DTO;

namespace Events.API.Interfaces
{
    public interface IParticipationsBL
    {
        public Task<IEnumerable<ParticipationDTO>> ObtenirParticipations();
        public Task<ParticipationDTO?> ObtenirSelonId(int id);
        public Task AjouterParticipation(ParticipationPostDTO requeteParticipation);
        public Task SupprimerParticipation(int id);
        public Task<IEnumerable<ParticipationDTO>> ObtenirParticipationsSelonEvenement(int idEvenement);
    }
}
