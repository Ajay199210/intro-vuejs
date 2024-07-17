using AutoMapper;
using Events.API.Data;
using Events.API.DTO;
using Events.API.Interfaces;
using Events.API.Interfaces.Repositories;

namespace Events.API.BusinessLogic
{
    public class StatisticsBL : IStatisticsBL
    {
        private readonly IEvenementsRepository _evenementsRepository;
        private readonly IEvenementsBL _evenementsBL;
        private readonly IVillesBL _villesBL;
        private readonly IMapper _mapper;
        private readonly ILogger<EventsDbContext> _logger;

        public StatisticsBL(IEvenementsRepository evenementsRepository, IEvenementsBL evenementsBL,
            IVillesBL villesBL, IMapper mapper, ILogger<EventsDbContext> logger)
        {
            _evenementsRepository = evenementsRepository;
            _evenementsBL = evenementsBL;
            _villesBL = villesBL;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<decimal> GetTotaleVentesEvenement(int id)
        {
            _logger.LogInformation($"[{DateTime.Now}] - Calcul en cours des ventes totales pour l'événement (id = {id})");

            return await _evenementsRepository.GetTotaleVentesEvenement(id);
        }

        public async Task<IEnumerable<EvenementDTO>> GetEvenementsRentables()
        {
            var evenements = await _evenementsRepository.ListAsync();
            var evenementsDTO = _mapper.Map<IList<EvenementDTO>>(evenements);

            foreach (var evenementDTO in evenementsDTO)
            {
                evenementDTO.TotaleVentes = await _evenementsRepository.GetTotaleVentesEvenement(evenementDTO.Id);
            }

            _logger.LogInformation($"[{DateTime.Now}] - Obtention de 10 événements les plus rentables...");

            return evenementsDTO.OrderByDescending(e => e.TotaleVentes).Take(10);
        }

        public async Task<IEnumerable<VilleStatsDTO>> GetVillesPopulaires()
        {
            var villesDTO = await _villesBL.ObtenirVilles();
            var villesStatsDTO = _mapper.Map<IList<VilleStatsDTO>>(villesDTO);

            foreach (var villeDTO in villesStatsDTO)
            {
                var evenementsVille = await _evenementsBL.ObtenirEvenementsSelonVille(villeDTO.Id);
                villeDTO.NbEvenements = evenementsVille.Count();
            }

            _logger.LogInformation($"[{DateTime.Now}] - Obtention de 10 villes les plus populaires...");

            return villesStatsDTO.OrderByDescending(v => v.NbEvenements).Take(10);
        }
    }
}
