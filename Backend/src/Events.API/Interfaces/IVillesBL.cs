using Events.API.DTO;

namespace Events.API.Interfaces
{
    public interface IVillesBL
    {
        public Task<IEnumerable<VilleDTO>> ObtenirVilles();
        public Task<VilleDTO?> ObtenirSelonId(int id);
        public Task AjouterVille(VillePostPutDTO requeteVille);
        public Task ModifierVille(int id, VillePostPutDTO ville);
        public Task SupprimerVille(int id);
    }
}
