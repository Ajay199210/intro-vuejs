using AutoMapper;
using Events.API.Data;
using Events.API.Data.Models;
using Events.API.DTO;
using Events.API.Exceptions;
using Events.API.Interfaces;
using Events.API.Interfaces.Repositories;

namespace Events.API.BusinessLogic
{
    public class CategoriesBL : ICategoriesBL
    {
        private readonly IAsyncRepository<Categorie> _categoriesRepository;
        private readonly IEvenementsRepository _evenementsRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<EventsDbContext> _logger;

        public CategoriesBL(IAsyncRepository<Categorie> categoriesRepository, IEvenementsRepository evenementsRepository,
            IMapper mapper, ILogger<EventsDbContext> logger)
        {
            _categoriesRepository = categoriesRepository;
            _evenementsRepository = evenementsRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task AjouterCategorie(CategoriePostPutDTO requeteCategorie)
        {

            var requeteCategorieV1 = _mapper.Map<CategorieDTO>(requeteCategorie);

            if (requeteCategorie == null)
            {
                throw new HttpException
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = new
                    {
                        errors = $"{DateTime.Now} [-] Paramètres d'entrée de la catégorie à ajouter sont non valides"
                    }
                };
            }

            var categories = await ObtenirCategories();
            var categorieExistante = categories
                .FirstOrDefault(c => c.Id == requeteCategorieV1.Id || c.Nom == requeteCategorieV1.Nom);

            if (categorieExistante != null)
            {
                throw new HttpException
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = new
                    {
                        Errors = $"{DateTime.Now} [-] Catégorie déjà existante (id = {categorieExistante.Id})"
                    }
                };
            }

            var categorie = _mapper.Map<Categorie>(requeteCategorie);

            _logger.LogInformation($"{DateTime.Now} [*] Ajout de la catégorie (id = {categorie.Id})...");

            await _categoriesRepository.AddAsync(categorie);
        }

        public async Task ModifierCategorie(int id, CategoriePostPutDTO requeteCategorie)
        {
            var requeteCategorieV1 = _mapper.Map<CategorieDTO>(requeteCategorie);

            if (requeteCategorie == null)
            {
                throw new HttpException
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = new
                    {
                        Errors = $"{DateTime.Now} [-] Paramètres d'entrée de la catégorie à modifier sont non valides"
                    }
                };
            }

            var categorieDTO = await ObtenirSelonId(id) ??
            throw new HttpException
            {
                StatusCode = StatusCodes.Status404NotFound,
                Errors = new
                {
                    Errors = $"{DateTime.Now} [-] Catégorie non trouvée (id = {id})"
                }
            };

            // Vérifier s'il y a une catégorie existante avec le même nom
            var categories = await ObtenirCategories();
            var categorieExistante = categories.
                FirstOrDefault(c => c.Id != requeteCategorieV1.Id && c.Nom == requeteCategorieV1.Nom);

            if (categorieExistante != null)
            {
                throw new HttpException
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = new
                    {
                        Errors = $"{DateTime.Now} [-] Catégorie déjà existante (id = {categorieExistante.Id})"
                    }
                };
            }

            var categorieAModifier = _mapper.Map<Categorie>(categorieDTO);

            categorieAModifier.Nom = requeteCategorie.Nom;

            _logger.LogInformation($"{DateTime.Now} [*] Modification de la catégorie (id = {categorieAModifier.Id})...");

            await _categoriesRepository.EditAsync(categorieAModifier);
        }

        public async Task<IEnumerable<CategorieDTO>> ObtenirCategories()
        {
            var categories = await _categoriesRepository.ListAsync();

            _logger.LogInformation($"{DateTime.Now} [*] Obtention de {categories.Count()} catégorie(s)...");

            return _mapper.Map<IList<CategorieDTO>>(categories);
        }

        public async Task<CategorieDTO?> ObtenirSelonId(int id)
        {
            var categorie = await _categoriesRepository.GetByIdAsync(id);

            _logger.LogInformation($"{DateTime.Now} [*] Obtention de la catégorie (id = {id})...");

            return _mapper.Map<CategorieDTO>(categorie);
        }

        public async Task SupprimerCategorie(int id)
        {
            // On ne peut pas supprimer une catégorie qui a au moins un événement associé
            var evenements = await _evenementsRepository.ListAsync();
            var evenementsDTO = _mapper.Map<IList<EvenementDTO>>(evenements);

            var categorieAssocieEvenement = evenementsDTO.Any(e => e.CategoriesIds.Any(catId => catId == id));

            if (categorieAssocieEvenement)
            {
                throw new HttpException
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = new
                    {
                        Errors = $"{DateTime.Now} [-] Cette catégorie est déjà associée " +
                    $"à un événement"
                    }
                };
            }

            var categorieDTO = await ObtenirSelonId(id) ??
            throw new HttpException
            {
                StatusCode = StatusCodes.Status404NotFound,
                Errors = new
                {
                    Errors = $"{DateTime.Now} [-] Catégorie non existante (id : {id})"
                }
            };

            var categorieASupprimer = _mapper.Map<Categorie>(categorieDTO);

            _logger.LogInformation($"{DateTime.Now} [*] Suppression de la catégorie (id = {id})...");

            await _categoriesRepository.DeleteAsync(categorieASupprimer);
        }
    }
}
