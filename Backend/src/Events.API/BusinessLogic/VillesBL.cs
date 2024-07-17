using AutoMapper;
using Events.API.Data;
using Events.API.Data.Enums;
using Events.API.Data.Models;
using Events.API.DTO;
using Events.API.Exceptions;
using Events.API.Interfaces;
using Events.API.Interfaces.Repositories;

namespace Events.API.BusinessLogic
{
    public class VillesBL : IVillesBL
    {
        private readonly IAsyncRepository<Ville> _villesRepository;
        private readonly IAsyncRepository<Evenement> _evenementsRepository;
        private readonly ILogger<EventsDbContext> _logger;
        private readonly IMapper _mapper;

        public VillesBL(IAsyncRepository<Ville> villesRepository, IAsyncRepository<Evenement> evenementsRepository,
            IMapper mapper, ILogger<EventsDbContext> logger)
        {
            _villesRepository = villesRepository;
            _evenementsRepository = evenementsRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task AjouterVille(VillePostPutDTO requeteVille)
        {
            if (requeteVille == null || string.IsNullOrEmpty(requeteVille.Nom))
            {
                throw new HttpException
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = new
                    {
                        Errors = $"{DateTime.Now} [-] Paramètres d'entrée de la ville à ajouter sont non valides"
                    }
                };
            }

            var villes = await ObtenirVilles();
            var villeExistante = villes
                .FirstOrDefault(v => v.Nom == requeteVille.Nom && v.Region == requeteVille.Region);

            if (villeExistante != null)
            {
                throw new HttpException
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = new
                    {
                        Errors = $"{DateTime.Now} [-] Ville déjà existante (ville id {villeExistante.Id})"
                    }
                };
            }

            // La ville doit correspondre à une région valide
            if (!Enum.IsDefined(typeof(Region), requeteVille.Region.ToString().ToUpper()))
            {
                throw new HttpException
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = new
                    {
                        Errors = $"{DateTime.Now} [-] Région non valide"
                    }
                };
            }

            var ville = _mapper.Map<Ville>(requeteVille);

            _logger.LogInformation($"{DateTime.Now} [*] Ajout de la ville (id = {ville.Id})...");

            await _villesRepository.AddAsync(ville);
        }

        public async Task ModifierVille(int id, VillePostPutDTO requeteVille)
        {
            if (requeteVille == null)
            {
                throw new HttpException
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = new
                    {
                        Errors = $"{DateTime.Now} [-] Paramètres d'entrée non valides de la ville à modifier"
                    }
                };
            }

            var villeDTO = await ObtenirSelonId(id);

            if (villeDTO == null)
            {
                throw new HttpException
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Errors = new
                    {
                        Errors = $"{DateTime.Now} [-] Ville non trouvée (id : {id})"
                    }
                };
            }

            // Vérifier s'il y a une ville existante avec le même nom et la même région
            var villes = await ObtenirVilles();
            var villeExistante = villes.FirstOrDefault(v => v.Id != id &&
            (v.Nom == requeteVille.Nom && v.Region == requeteVille.Region));

            if (villeExistante != null)
            {
                throw new HttpException
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = new
                    {
                        Errors = $"{DateTime.Now} [-] Ville déjà existante (id = {villeExistante.Id})"
                    }
                };
            }

            var villeAModifier = _mapper.Map<Ville>(villeDTO);

            villeAModifier.Nom = requeteVille.Nom;
            villeAModifier.Region = requeteVille.Region;

            _logger.LogInformation($"{DateTime.Now} [*] Modification de la ville (id = {villeAModifier.Id})...");

            await _villesRepository.EditAsync(villeAModifier);
        }

        public async Task<VilleDTO?> ObtenirSelonId(int id)
        {
            var ville = await _villesRepository.GetByIdAsync(id);

            _logger.LogInformation($"{DateTime.Now} [*] Obtention de la ville (id = {id})...");

            return _mapper.Map<VilleDTO>(ville);
        }

        public async Task<IEnumerable<VilleDTO>> ObtenirVilles()
        {
            var villes = await _villesRepository.ListAsync();
            var villesDTO = _mapper.Map<IList<VilleDTO>>(villes);

            _logger.LogInformation($"{DateTime.Now} [*] Obtention de {villesDTO.Count()} villes...");

            return villesDTO;
        }

        public async Task SupprimerVille(int id)
        {
            // On ne peut pas supprimer une ville qui a au moins un événement associé
            var evenements = await _evenementsRepository.ListAsync();
            var villeAssocieeEvenement = evenements.Any(e => e.VilleId == id);

            if (villeAssocieeEvenement)
            {
                throw new HttpException
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = new { Errors = $"{DateTime.Now} [-] Cette ville est déjà associée à un événement" }
                };
            }

            var villeDTO = await ObtenirSelonId(id);

            if (villeDTO == null)
            {
                throw new HttpException
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Errors = new
                    {
                        Errors = $"{DateTime.Now} [-] Ville non existante (id : {id})"
                    }
                };
            }

            var villeASupprimer = _mapper.Map<Ville>(villeDTO);

            _logger.LogInformation($"{DateTime.Now} [*] Suppression de la ville (id = {id})...");

            await _villesRepository.DeleteAsync(villeASupprimer);
        }
    }
}
