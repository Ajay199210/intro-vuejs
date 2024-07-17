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
    public class VillesController : ControllerBase
    {
        private readonly IVillesBL _villesBL;

        public VillesController(IVillesBL villesBL)
        {
            _villesBL = villesBL;
        }

        // GET: api/Villes
        /// <summary>
        /// Obtenir la liste des villes enregistrées
        /// </summary>
        /// <remarks> Request Sample:
        ///    GET api/villes
        /// </remarks>
        /// <returns>La liste des villes</returns>
        /// <response code="200">Liste retournée en succès</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<VilleDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VilleDTO>>> Get()
        {
            var villes = await _villesBL.ObtenirVilles();
            return Ok(villes);
        }

        /// GET api/Villes/5
        /// <summary>
        /// Obtenir les détails d'une ville à partir de son id
        /// </summary>
        /// <param name="id">Identifiant de la ville</param>
        /// <returns><see cref="VilleDTO"/></returns>
        /// <response code="200">Ville trouvée</response>
        /// <response code="404">Ville introuvable</response>
        /// <response code="500">Erreur du côté serveur</response> 
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(VilleDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<VilleDTO>>> GetById(int id)
        {
            var ville = await _villesBL.ObtenirSelonId(id);

            return ville == null ?
                NotFound(new { Erreur = $"Ville introuvable (id = {id})" }) : Ok(ville);
        }

        /// POST api/Villes
        /// <summary>
        /// Permet d'ajouter une ville
        /// </summary>
        /// <param name="requeteVille">Ville (<see cref="VillePostPutDTO"/>) à ajouter</param>
        /// <returns>La nouvelle ville qui est ajoutée</returns>
        /// <response code="201">Ville ajoutée avec succès</response>
        /// <response code="400">Model Invalide, mauvaise requête</response>
        /// <response code="500">Service indisponible pour le moment</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(VillePostPutDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Post([FromBody] VillePostPutDTO requeteVille)
        {
            await _villesBL.AjouterVille(requeteVille);
            return CreatedAtAction("Post", null);
        }

        // PUT api/Villes/5
        /// <summary>
        /// Permet la modification d'une ville
        /// </summary>
        /// <param name="id">L'id dels la ville</param>
        /// <param name="requeteVille">La requête ville</param>
        /// <returns>Ville modifiée dans la liste ou BD</returns>
        /// <response code="204">Ville modifiée avec succès, aucun contenu retourné</response>
        /// <response code="400">Mauvaise requête, model invalide</response>
        /// <response code="404">Ville introuvable pour l'id spécifié</response>
        /// <response code="500">service indisponible pour le moment</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(VilleDTO), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Put(int id, [FromBody] VillePostPutDTO requeteVille)
        {
            await _villesBL.ModifierVille(id, requeteVille);
            return NoContent();
        }

        // DELETE api/Villes/5
        /// <summary>
        /// Permet la suppression d'une ville
        /// </summary>
        /// <param name="id">Id de la ville à supprimer</param>
        /// <response code="204">Ville supprimée avec succès, aucun contenu retourné</response>
        /// <response code="400">Mauvaise requête : la ville est déjà associée à un événement</response>
        /// <response code="404">Ville introuvable pour l'id spécifié</response>
        /// <response code="500">Service indisponible pour le moment</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(VilleDTO), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Delete(int id)
        {
            await _villesBL.SupprimerVille(id);
            return NoContent();
        }
    }
}
