using AutoMapper;
using Events.API.Data.Models;
using Events.API.DTO;
using Events.API.Interfaces;
using Events.API.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects,
// visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Events.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ParticipationsController : ControllerBase
    {
        private readonly IParticipationsBL _participationsBL;
        private readonly IParticipationsRepository _participationsRepository;
        private readonly IMapper _mapper;

        public ParticipationsController(IParticipationsBL participationsBL, IParticipationsRepository participationsRepository,
            IMapper mapper)
        {
            _participationsBL = participationsBL;
            _participationsRepository = participationsRepository;
            _mapper = mapper;
        }

        // GET: api/Participations
        /// <summary>
        /// Obtenir la liste des participations enregistrées
        /// </summary>
        /// <remarks> Request sample:
        ///    GET api/participations
        /// </remarks>
        /// <returns>La liste des participations</returns>
        /// <response code="200">Liste retournée en succès</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<ParticipationDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ParticipationDTO>>> Get()
        {
            // Lister toutes les participations pour un événement donné
            var participations = await _participationsBL.ObtenirParticipations();
            return Ok(participations);
        }

        // GET api/Participation/5
        /// <summary>
        /// Obtenir les détails d'une participation à partir de son id
        /// </summary>
        /// <param name="id">Identifiant de la participation</param>
        /// <returns><see cref="ParticipationDTO"/></returns>
        /// <response code="200">Participation trouvée</response>
        /// <response code="404">Participation introuvable</response>
        /// <response code="500">Erreur du côté serveur</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ParticipationDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Participation>> GetById(int id)
        {
            var participation = await _participationsBL.ObtenirSelonId(id);

            return participation == null ?
                NotFound(new { Erreur = $"Participation introuvable (id = {id})" }) : Ok(participation);
        }

        // POST api/Participations
        /// <summary>
        /// Permet d'ajouter une participation
        /// </summary>
        /// <param name="requeteParticipation">Participation (<see cref="ParticipationPostDTO"/>) à ajouter</param>
        /// <returns>La nouvelle participation qui est ajoutée</returns>
        /// <response code="202">Requête reçue mais le traitement côté serveur n'est pas terminé</response>
        /// <response code="400">Mauvaise requête</response>
        /// <response code="500">Service indisponible pour le moment</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(ParticipationPostDTO), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(ParticipationPostDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ParticipationPostDTO), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] ParticipationPostDTO requeteParticipation)
        {
            if (ModelState.IsValid)
            {
                await _participationsBL.AjouterParticipation(requeteParticipation);
                var participationDTO = _mapper.Map<ParticipationDTO>(requeteParticipation);

                // HTTP 202(Accepted) puisque il reste la confirmation externe de la participation
                return new AcceptedResult { Location = Url.Action(nameof(Status), new { id = participationDTO.Id }) };
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Retourne le status de la participation à partir de son id
        /// </summary>
        /// <param name="id">Id de la participation en question</param>
        /// <remarks>
        ///     GET /Participations/1/Status
        /// </remarks>
        /// <response code="200">Traitement executé avec succès, contenu retourné</response>
        /// <response code="303">
        ///     La redirection ne fait pas le lien vers la ressource nouvellement téléversée 
        ///     mais vers une autre page (see https://http.dev/303). 
        ///     La méthode utilisée pour afficher la page redirigée est toujours GET
        /// </response>
        /// <response code="404">Participation introuvable pour l'id spécifié</response>
        /// <response code="500">Service indisponible pour le moment</response>
        [HttpGet("{id}/status")]
        [ProducesResponseType(typeof(Participation), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status303SeeOther)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Status(int id)
        {
            var participation = await _participationsBL.ObtenirSelonId(id);

            if (participation == null)
            {
                return NotFound();
            }

            simulerConfirmationParticipation(participation);

            if (participation.EstConfirmee)
            {
                // Enregistrer la confirmation dans la BD
                var participationAModifier = _mapper.Map<Participation>(participation);
                await _participationsRepository.EditAsync(participationAModifier);

                Response.Headers.Add("Location", Url.Action(nameof(GetById), new { id }));

                return new StatusCodeResult(StatusCodes.Status303SeeOther);
            }

            return Ok(new { status = "Validation en attente" });
        }

        // Simuler la confirmation d'une participation
        private void simulerConfirmationParticipation(ParticipationDTO requeteParticipation)
        {
            if (!requeteParticipation.EstConfirmee)
            {
                requeteParticipation.EstConfirmee = new Random().Next(1, 10) > 5 ? true : false;
            }
        }

        // DELETE api/Participations/5
        /// <summary>
        /// Permet la suppression d'une participation
        /// </summary>
        /// <param name="id">Id de la participation à supprimer</param>
        /// <response code="204">Participation supprimée avec succès, aucun contenu retourné</response>
        /// <response code="404">Participation introuvable pour l'id spécifié</response>
        /// <response code="500">Service indisponible pour le moment</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ParticipationDTO), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            await _participationsBL.SupprimerParticipation(id);

            return NoContent();
        }

        // GET: api/Participations/Evenements/5/Participations
        /// <summary>
        /// Obtenir la liste des participations selon l'id de l'événement donné en paramètre
        /// </summary>
        /// <param name="id">Identifiant de l'événement pour appliquer le filtre</param>
        /// <remarks> Sample of request:
        ///    GET api/Participations/Evenements/5/Participations
        /// </remarks>
        /// <returns>La liste des participations pour l'événement</returns>
        /// <response code="200">Liste retournée en succès</response>
        [HttpGet("~/api/Evenements/{id}/Participations")]
        [ProducesResponseType(typeof(List<EvenementDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EvenementDTO>>> Get(int id)
        {
            return Ok(await _participationsBL.ObtenirParticipationsSelonEvenement(id));
        }
    }
}
