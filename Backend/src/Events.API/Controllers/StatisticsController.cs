using Events.API.DTO;
using Events.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Events.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsBL _statisticsBL;

        public StatisticsController(IStatisticsBL statisticsBL)
        {
            _statisticsBL = statisticsBL;
        }

        // GET: api/Statistics/Evenements/{id}/GetTotalVentes
        /// <summary>Retourner le total des ventes d'un événement</summary>
        /// <remarks> Request Sample:
        ///    GET api/Statistics/Evenements/5/GetTotalVentes
        /// </remarks>
        /// <returns>Le total des ventes</returns>
        /// <response code="200">Liste retournée en succès</response>
        [HttpGet("Evenements/{id}/GetTotalVentes")]
        [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
        public async Task<ActionResult<decimal?>> GetTotaleVentes(int id)
        {
            var ventesTotalesEvenement = await _statisticsBL.GetTotaleVentesEvenement(id);
            return Ok(ventesTotalesEvenement);
        }

        // GET: api/Statistics/Villes/GetTotalVentes
        /// <summary>Retourner les 10 villes les plus populaires</summary>
        /// <remarks> Request Sample:
        ///    GET api/Statistics/Villes/GetTotalVentes
        /// </remarks>
        /// <returns>La liste des villes populaires</returns>
        /// <response code="200">Liste retournée en succès</response>
        [HttpGet("Villes/GetVillesPopulaires")]
        [ProducesResponseType(typeof(List<VilleStatsDTO>), StatusCodes.Status200OK)]
        public async Task<IEnumerable<VilleStatsDTO>> GetVillesPopulaires()
        {
            return await _statisticsBL.GetVillesPopulaires();
        }

        // GET: api/Statistics/Evenements/GetEvenementsRentables
        /// <summary>Retourner les 10 événements les plus rentables</summary>
        /// <remarks> Request Sample:
        ///    GET api/Statistics/Evenements/GetEvenementsRentables
        /// </remarks>
        /// <returns>La liste des événements rentables</returns>
        /// <response code="200">Liste retournée en succès</response>
        [HttpGet("Evenements/GetEvenementsRentables")]
        [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
        public async Task<IEnumerable<EvenementDTO>> GetEvenementsRentables()
        {
            return await _statisticsBL.GetEvenementsRentables();
        }
    }
}
