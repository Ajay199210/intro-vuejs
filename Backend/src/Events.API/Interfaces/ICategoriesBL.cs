using Events.API.DTO;

namespace Events.API.Interfaces
{
    public interface ICategoriesBL
    {
        public Task<IEnumerable<CategorieDTO>> ObtenirCategories();
        public Task<CategorieDTO?> ObtenirSelonId(int id);
        public Task AjouterCategorie(CategoriePostPutDTO categorie);
        public Task ModifierCategorie(int id, CategoriePostPutDTO categorie);
        public Task SupprimerCategorie(int id);
    }
}
