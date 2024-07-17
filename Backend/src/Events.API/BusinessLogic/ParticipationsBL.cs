using AutoMapper;
using Events.API.Data;
using Events.API.Data.Models;
using Events.API.DTO;
using Events.API.Exceptions;
using Events.API.Interfaces;
using Events.API.Interfaces.Repositories;

namespace Events.API.BusinessLogic
{
    public class ParticipationsBL : IParticipationsBL
    {
        private readonly IEvenementsRepository _evenementsRepository;
        private readonly IParticipationsRepository _participationsRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<EventsDbContext> _logger;

        public ParticipationsBL(IEvenementsRepository evenementsRepository, IParticipationsRepository participationsRepository,
            IMapper mapper, ILogger<EventsDbContext> logger)
        {
            _evenementsRepository = evenementsRepository;
            _participationsRepository = participationsRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task AjouterParticipation(ParticipationPostDTO requeteParticipation)
        {
            if (requeteParticipation == null)
            {
                throw new HttpException
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = new
                    {
                        Errors = $"{DateTime.Now} [-] Paramètres d'entrée de la participation à ajouter non valides"
                    }
                };
            }

            // Vérifier si l'événement existe avant la participation
            var evenements = await _evenementsRepository.ListAsync();
            if (evenements.All(e => e.Id != requeteParticipation.EvenementId))
            {
                throw new HttpException
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = new
                    {
                        Errors = $"{DateTime.Now} [-] L'événement (id = {requeteParticipation.EvenementId}) doit existe pour participer"
                    }
                };
            }

            // On ne peut pas participer plus d'une fois à un même événement avec le même adresse courriel
            var participations = await _participationsRepository.ListAsync();
            var courrielEstUtilise = participations.Any(p => p.EvenementId == requeteParticipation.EvenementId &&
                p.AdresseCourriel.ToLower() == requeteParticipation.AdresseCourriel.ToLower());

            if (courrielEstUtilise)
            {
                throw new HttpException
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = new
                    {
                        Errors = $"{DateTime.Now} [-] L'adresse courriel est déjà utilisé" +
                    $" pour une autre participation"
                    }
                };
            }

            // On doit avoir au moins une place pour participer
            if (requeteParticipation.NbPlaces <= 0)
            {
                throw new HttpException
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = new
                    {
                        Errors = $"{DateTime.Now} [-] Vous devez réserver au moins une place pour participer"
                    }
                };
            }

            var participation = _mapper.Map<Participation>(requeteParticipation);

            _logger.LogInformation($"{DateTime.Now} [*] Ajout de la participation (id = {participation.Id})...");

            await _participationsRepository.AddAsync(participation);

        }

        public async Task<IEnumerable<ParticipationDTO>> ObtenirParticipations()
        {
            var participations = await _participationsRepository.ListAsync();
            var participationsDTO = _mapper.Map<IList<ParticipationDTO>>(participations).Where(p => p.EstConfirmee);

            _logger.LogInformation($"{DateTime.Now} [*] Obtention de {participationsDTO.Count()} participations...");

            return participationsDTO;
        }

        public async Task<ParticipationDTO?> ObtenirSelonId(int id)
        {
            var participation = await _participationsRepository.GetByIdAsync(id);

            _logger.LogInformation($"{DateTime.Now} [*] Obtention de la participation (id = {id})...");

            return _mapper.Map<ParticipationDTO>(participation);
        }

        public async Task SupprimerParticipation(int id)
        {
            var participationDTO = await ObtenirSelonId(id) ?? throw new HttpException
            {
                StatusCode = StatusCodes.Status404NotFound,
                Errors = new
                {
                    Errors = $"{DateTime.Now} [-] Participation non trouvable (id = {id})"
                }
            };
            var participationASupprimer = _mapper.Map<Participation>(participationDTO);

            _logger.LogInformation($"{DateTime.Now} [*] Suppression de la participation (id = {id})...");

            await _participationsRepository.DeleteAsync(participationASupprimer);
        }

        public async Task<IEnumerable<ParticipationDTO>> ObtenirParticipationsSelonEvenement(int idEvenement)
        {
            var participations = await _participationsRepository.ListAsync();
            var participationsDTO = _mapper.Map<IList<ParticipationDTO>>(participations);
            var participationsEvenement = participationsDTO.ToList().FindAll(p => p.EvenementId == idEvenement);

            _logger.LogInformation($"{DateTime.Now} [*] Obtention de {participationsEvenement.Count()} participations disponibles " +
                $"pour l'événement (id = {idEvenement})");

            return participationsEvenement;
        }
    }
}
