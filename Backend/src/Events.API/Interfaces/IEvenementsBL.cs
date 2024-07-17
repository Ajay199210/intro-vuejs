using Events.API.DTO;

namespace Events.API.Interfaces
{
    public interface IEvenementsBL
    {
        public Task<IEnumerable<EvenementDTO>> ObtenirEvenements();
        public Task<EvenementDTO?> ObtenirSelonId(int id);
        public Task AjouterEvenement(EvenementPostPutDTO requeteEvenement);
        public Task ModifierEvenement(int id, EvenementPostPutDTO evenement);
        public Task SupprimerEvenement(int id);
        public Task<IEnumerable<EvenementDTO>> ObtenirEvenementsSelonVille(int idVille);
    }
}
