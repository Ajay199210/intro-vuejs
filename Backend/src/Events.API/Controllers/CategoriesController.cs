using Events.API.DTO;
using Events.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

//For more information on enabling Web API for empty projects,
//visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Events.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesBL _categoriesBL;

        public CategoriesController(ICategoriesBL categoriesBL)
        {
            _categoriesBL = categoriesBL;
        }

        // GET: api/Categories
        /// <summary>
        /// Obtenir la liste des catégories enregistrées
        /// </summary>
        /// <remarks> Sample of request:
        ///    GET api/categories
        /// </remarks>
        /// <returns>La liste des catégories</returns>
        /// <response code="200">Liste retournée en succès</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<CategorieDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CategorieDTO>>> Get()
        {
            var categories = await _categoriesBL.ObtenirCategories();
            return Ok(categories);
        }

        // GET api/Categories/5
        /// <summary>
        /// Obtenir les detail d'une catégorie à partir de son id
        /// </summary>
        /// <param name="id">Identifiant de la catégorie</param>
        /// <returns><see cref="CategorieDTO"/></returns>
        /// <response code="200">Catégorie trouvée</response>
        /// <response code="404">Catégorie introuvable</response>
        /// <response code="500">Erreur du côté serveur</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CategorieDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CategorieDTO>> GetById(int id)
        {
            var categorie = await _categoriesBL.ObtenirSelonId(id);

            return categorie == null ?
                NotFound(new { Erreur = $"Categorie introuvable (id = {id})" }) : Ok(categorie);
        }

        // POST api/Categories
        /// <summary>
        /// Permet d'ajouter une catégorie
        /// </summary>
        /// <param name="requeteCategorie">Catégorie (<see cref="CategoriePostPutDTO"/>) à ajouter</param>
        /// <returns>La nouvelle catégorie qui est ajoutée</returns>
        /// <response code="201">Catégorie ajoutée avec succès</response>
        /// <response code="400">Model Invalide, mauvaise requête</response>
        /// <response code="500">Service indisponible pour le moment</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(CategoriePostPutDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CategoriePostPutDTO requeteCategorie)
        {
            if (ModelState.IsValid)
            {
                await _categoriesBL.AjouterCategorie(requeteCategorie);

                //return CreatedAtAction(nameof(GetById), new { id = requeteCategorie.Id }, null);
                return CreatedAtAction("Post", null);
            }

            return BadRequest(ModelState);
        }

        // PUT api/Categories/5
        /// <summary>
        /// Permet la modification d'une catégorie
        /// </summary>
        /// <param name="id">L'id de la ville</param>
        /// <param name="requeteCategorie"></param>
        /// <returns>Categorie modifiée dans la liste ou BD</returns>
        /// <response code="204">Catégorie modifiée avec succès, aucun contenu retourné</response>
        /// <response code="404">Catégorie introuvable pour l'id spécifié</response>
        /// <response code="500">service indisponible pour le moment</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CategoriePostPutDTO), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int id, [FromBody] CategoriePostPutDTO requeteCategorie)
        {
            if (ModelState.IsValid)
            {
                await _categoriesBL.ModifierCategorie(id, requeteCategorie);
                return NoContent();
            }

            return BadRequest(ModelState);
        }

        // DELETE api/Categories/5
        /// <summary>
        /// Permet la suppression d'une catégorie
        /// </summary>
        /// <param name="id">Id de la catégorie à supprimer</param>
        /// <response code="204">Catégorie supprimée avec succès, aucun contenu retourné</response>
        /// <response code="400">Mauvaise requête : La catégorie en question est déjà associée à un événement</response>
        /// <response code="404">Catégorie introuvable pour l'id spécifié</response>
        /// <response code="500">Service indisponible pour le moment</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(CategorieDTO), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoriesBL.SupprimerCategorie(id);
            return NoContent();
        }
    }
}
