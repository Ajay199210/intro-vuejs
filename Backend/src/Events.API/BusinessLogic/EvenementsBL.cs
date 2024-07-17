using AutoMapper;
using Events.API.Data;
using Events.API.Data.Models;
using Events.API.DTO;
using Events.API.Exceptions;
using Events.API.Interfaces;
using Events.API.Interfaces.Repositories;

namespace Events.API.BusinessLogic
{
    public class EvenementsBL : IEvenementsBL
    {
        private readonly IEvenementsRepository _evenementsRepository;
        private readonly IAsyncRepository<Categorie> _categoriesRepository;
        private readonly IAsyncRepository<Ville> _villesRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<EventsDbContext> _logger;

        public EvenementsBL(IEvenementsRepository evenementsRepository, IAsyncRepository<Categorie> categoriesRepository,
            IAsyncRepository<Ville> villesRepository, IMapper mapper, ILogger<EventsDbContext> logger)
        {
            _evenementsRepository = evenementsRepository;
            _categoriesRepository = categoriesRepository;
            _villesRepository = villesRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task AjouterEvenement(EvenementPostPutDTO requeteEvenement)
        {
            // Vérifier que les champs sont valides
            if (requeteEvenement == null)
            {
                throw new HttpException
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = new
                    {
                        Errors = $"{DateTime.Now} [-] Paramètres d'entrée non valides"
                    }
                };
            }

            if (requeteEvenement.DateDebut > requeteEvenement.DateFin)
            {
                throw new HttpException
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = new
                    {
                        Errors = $"{DateTime.Now} [-] La date de début d'un évènement " +
                    $"ne peut pas être après sa date de fin"
                    }
                };
            }

            // Vérifier si la ville existe avant l'ajout de l'événement
            var ville = await _villesRepository.GetByIdAsync(requeteEvenement.VilleId);
            var villeDTO = _mapper.Map<VilleDTO>(ville) ??
            throw new HttpException
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Errors = new
                {
                    Errors = $"{DateTime.Now} [-] La ville doit exister pour l'ajout de l'événement"
                }
            };

            // Vérifier si tous les catégories existent avant l'ajout de l'événement
            var categories = await _categoriesRepository.ListAsync();
            var categoriesDTO = _mapper.Map<IList<CategorieDTO>>(categories);

            var categoriesNonVerifiees = requeteEvenement.CategoriesIds
                .Any(id => categoriesDTO.All(c => c.Id != id));

            if (categoriesNonVerifiees)
            {
                throw new HttpException
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = new
                    {
                        Errors = $"{DateTime.Now} [-] Tous les catégories spécifiées doivent existent " +
                                $"avant l'ajout de l'événement"
                    }
                };
            }

            // L'événement ne peut avoir une catégories répétées plus qu'une fois
            foreach (var idCategorie in requeteEvenement.CategoriesIds)
            {
                if (requeteEvenement.CategoriesIds.Count(idCat => idCat == idCategorie) > 1)
                {
                    throw new HttpException
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Errors = new
                        {
                            Errors = $"{DateTime.Now} [-] Une catégorie ne peut être répétée deux fois"
                        }
                    };
                }
            }

            var evenement = _mapper.Map<Evenement>(requeteEvenement);

            _logger.LogInformation($"{DateTime.Now} [*] Ajout de l'événement (id = {evenement.Id})...");

            await _evenementsRepository.AddAsync(evenement);
        }

        public async Task ModifierEvenement(int id, EvenementPostPutDTO requeteEvenement)
        {
            if (requeteEvenement == null)
            {
                throw new HttpException
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = new
                    {
                        Errors = $"{DateTime.Now} [-] Paramètres d'entrée de l'événement " +
                        $"à modifier sont non valides"
                    }
                };
            }

            var evenementDTO = await ObtenirSelonId(id) ??
            throw new HttpException
            {
                StatusCode = StatusCodes.Status404NotFound,
                Errors = new
                {
                    Errors = $"{DateTime.Now} [-] Événement non trouvable (id = {id})"
                }
            };

            if (requeteEvenement.DateDebut > requeteEvenement.DateFin)
            {
                throw new HttpException
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = new
                    {
                        Errors = $"{DateTime.Now} [-] La date de début d'un évènement " +
                    $"ne peut pas être après sa date de fin"
                    }
                };
            }

            // Vérifier si la ville existe avant l'ajout de l'événement
            var villes = await _villesRepository.ListAsync();
            var villesDTO = _mapper.Map<IList<VilleDTO>>(villes);

            if (villesDTO.All(v => v.Id != requeteEvenement.VilleId))
            {
                throw new HttpException
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = new
                    {
                        Errors = $"{DateTime.Now} [-] La ville doit exister avant la modification de l'événement"
                    }
                };
            }

            // Vérifier si tous les catégories existent avant l'ajout de l'événement
            var categories = await _categoriesRepository.ListAsync();
            var categoriesDTO = _mapper.Map<IList<CategorieDTO>>(categories);

            var categoriesNonVerifiees = requeteEvenement.CategoriesIds
                .Any(id => categoriesDTO.All(c => c.Id != id));

            if (categoriesNonVerifiees)
            {
                throw new HttpException
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = new
                    {
                        Errors = $"{DateTime.Now} [-] Tous les catégories spécifiées doivent existent " +
                                $"avant l'ajout de l'événement"
                    }
                };
            }

            // L'événement ne peut avoir une catégories répétées plus qu'une fois
            foreach (var idCategorie in requeteEvenement.CategoriesIds)
            {
                if (requeteEvenement.CategoriesIds.Count(idCat => idCat == idCategorie) > 1)
                {
                    throw new HttpException
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Errors = new
                        {
                            Errors = $"{DateTime.Now} [-] Une catégorie ne peut être répétée deux fois"
                        }
                    };
                }
            }

            var evenementAModifier = _mapper.Map<Evenement>(evenementDTO);

            // Appliquer les modifications
            evenementAModifier.Titre = requeteEvenement.Titre;
            evenementAModifier.NomOrganisateur = requeteEvenement.NomOrganisateur;
            evenementAModifier.CategoriesIds = requeteEvenement.CategoriesIds;
            evenementAModifier.Adresse = requeteEvenement.Adresse;
            evenementAModifier.DateDebut = requeteEvenement.DateDebut;
            evenementAModifier.DateFin = requeteEvenement.DateFin;
            evenementAModifier.PrixBillet = requeteEvenement.PrixBillet;
            evenementAModifier.VilleId = requeteEvenement.VilleId;

            _logger.LogInformation($"{DateTime.Now} [*] Modification de l'événement (id = {evenementAModifier.Id})...");

            await _evenementsRepository.EditAsync(evenementAModifier);
        }

        public async Task<IEnumerable<EvenementDTO>> ObtenirEvenements()
        {
            var evenements = await _evenementsRepository.ListAsync();

            var evenementsDTO = _mapper.Map<IList<EvenementDTO>>(evenements);
            foreach (var evenementTDO in evenementsDTO)
            {
                evenementTDO.TotaleVentes = await _evenementsRepository.GetTotaleVentesEvenement(evenementTDO.Id);
            }

            _logger.LogInformation($"{DateTime.Now} [*] Obtention de {evenementsDTO.Count} événement(s)...");

            return evenementsDTO;
        }

        public async Task<EvenementDTO?> ObtenirSelonId(int id)
        {
            var evenement = await _evenementsRepository.GetByIdAsync(id);
            var evenementDTO = _mapper.Map<EvenementDTO>(evenement);
            evenementDTO.TotaleVentes = await _evenementsRepository.GetTotaleVentesEvenement(id);

            _logger.LogInformation($"{DateTime.Now} [*] Obtention de l'événement (id = {id})...");

            return evenementDTO;
        }

        public async Task SupprimerEvenement(int id)
        {
            var evenementDTO = await ObtenirSelonId(id) ?? throw new HttpException
            {
                StatusCode = StatusCodes.Status404NotFound,
                Errors = new
                {
                    Errors = $"{DateTime.Now} [-] Événement non trouvable (id = {id})"
                }
            };

            var evenementASupprimer = _mapper.Map<Evenement>(evenementDTO);

            _logger.LogInformation($"{DateTime.Now} [*] Suppression de l'événement (id = {id})...");

            await _evenementsRepository.DeleteAsync(evenementASupprimer);
        }

        public async Task<IEnumerable<EvenementDTO>> ObtenirEvenementsSelonVille(int idVille)
        {
            var evenements = await ObtenirEvenements();
            var evenementsVille = evenements.ToList().FindAll(e => e.VilleId == idVille);

            _logger.LogInformation($"{DateTime.Now} [*] Obtention de {evenementsVille.Count()} événements disponibles " +
                $"pour la ville (id = {idVille})");

            return evenementsVille;
        }
    }
}
