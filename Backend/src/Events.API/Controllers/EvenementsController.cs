using Events.API.DTO;
using Events.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects,
// visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Events.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class EvenementsController : ControllerBase
    {
        private readonly IEvenementsBL _evenementsBL;

        public EvenementsController(IEvenementsBL evenementsBL)
        {
            _evenementsBL = evenementsBL;
        }

        // GET: api/Evenements
        /// <summary>
        /// Obtenir la liste des événements enregistrés
        /// avec la possibilité de filtrer les événements
        /// </summary>
        /// <remarks> Sample of request:
        ///    GET api/evenements
        /// </remarks>
        /// <returns>La liste des événements</returns>
        /// <response code="200">Liste retournée en succès</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EvenementDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EvenementDTO>>> Get(string? searchTerm, int pageIndex = 1, int pageCount = 10)
        {
            var evenements = await _evenementsBL.ObtenirEvenements();
            evenements = evenements.Skip((pageIndex - 1) * pageCount).Take(pageCount).OrderBy(e => e.DateDebut);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                evenements = evenements.ToList()
                    .FindAll(e => e.Titre.ToLower().Contains(searchTerm) || e.Description.ToLower().Contains(searchTerm));
            }

            return Ok(evenements);
        }

        // GET api/Evenements/5
        /// <summary>
        /// Obtenir les détails d'un événement à partir de son id
        /// </summary>
        /// <param name="id">Identifiant de l'événement</param>
        /// <returns><see cref="EvenementDTO"/></returns>
        /// <response code="200">Element trouvé</response>
        /// <response code="404">Element introuvable</response>
        /// <response code="500">Erreur du service (Internal Server Error)</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EvenementDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EvenementDTO>> GetById(int id)
        {
            var evenement = await _evenementsBL.ObtenirSelonId(id);

            return evenement == null ?
                NotFound(new { Erreur = $"Événement introuvable (id = {id})" }) : Ok(evenement);
        }

        // GET: api/Evenements/Villes/5/Evenements
        /// <summary>
        /// Obtenir la liste des événements selon l'id de la ville donné en paramètre
        /// </summary>
        /// <param name="id">Identifiant de la ville pour appliquer le filtre</param>
        /// <remarks> Sample of request:
        ///    GET api/Evenements/Villes/5/Evenements
        /// </remarks>
        /// <returns>La liste des événements pour la ville</returns>
        /// <response code="200">Liste retournée en succès</response>
        [HttpGet("~/api/Villes/{id}/Evenements")]
        [ProducesResponseType(typeof(List<EvenementDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EvenementDTO>>> Get(int id)
        {
            return Ok(await _evenementsBL.ObtenirEvenementsSelonVille(id));
        }

        // POST api/Evenements
        /// <summary>
        /// Permet d'ajouter un événement
        /// </summary>
        /// <param name="requeteEvenement">Événement (<see cref="EvenementPostPutDTO"/>) à ajouter</param>
        /// <returns>Le nouveau événement ajouté</returns>
        /// <response code="201">Événement ajouté avec succès</response>
        /// <response code="400">Mauvaise requête : La date du début dépasse la date de fin</response>
        /// <response code="500">Service indisponible pour le moment</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(EvenementPostPutDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(EvenementPostPutDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = "ManagerPolicy")]
        public async Task<IActionResult> Post([FromBody] EvenementPostPutDTO requeteEvenement)
        {
            if (ModelState.IsValid)
            {
                await _evenementsBL.AjouterEvenement(requeteEvenement);
                //return CreatedAtAction(nameof(GetById), new { id = requeteEvenement.Id }, null);
                return CreatedAtAction("Post", null);
            }

            return BadRequest(ModelState);
        }

        // PUT api/Evenements/5
        /// <summary>
        /// Permet la modification d'un événement
        /// </summary>
        /// <param name="id">id de l'événement à modifier</param>
        /// <param name="requeteEvenement"></param>
        /// <returns>Événement modifié dans la liste ou BD</returns>
        /// <response code="204">Évenement modifié avec succès, aucun contenu retourné</response>
        /// <response code="400">Mauvaise requête (côté client)</response>
        /// <response code="404">Évenement introuvable pour l'id spécifié</response>
        /// <response code="500">service indisponible pour le moment</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(EvenementPostPutDTO), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = "ManagerPolicy")]
        public async Task<IActionResult> Put(int id, [FromBody] EvenementPostPutDTO requeteEvenement)
        {
            if (ModelState.IsValid)
            {
                await _evenementsBL.ModifierEvenement(id, requeteEvenement);

                return NoContent();
            }

            return BadRequest(ModelState);
        }

        // DELETE api/Evenements/5
        /// <summary>
        /// Permet la suppression d'un événement
        /// </summary>
        /// <param name="id">Id de l'événement à supprimer</param>
        /// <response code="204">Événement supprimé avec succès, aucun contenu retourné</response>
        /// <response code="404">Événement introuvable pour l'id spécifié</response>
        /// <response code="500">Service indisponible pour le moment</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(EvenementDTO), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = "ManagerPolicy")]
        public async Task<IActionResult> Delete(int id)
        {
            await _evenementsBL.SupprimerEvenement(id);

            return NoContent();
        }
    }
}
