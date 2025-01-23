using LibrIO.Classes;
using LibrIO.Classes_DTO;
using LibrIO.Data;
using LibrIO.Methode;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace LibrIO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly LibrIODb _dbLivre;
        // eviter la valeur Null 
        public GenreController(LibrIODb librioDB)
        {
            _dbLivre = librioDB;
        }
        // Créer un genre 
        [HttpPost("api/PostGenre")]
        [SwaggerOperation(
    Summary = "créer un Genre",
    Description = "Ici pourras etre uniquement saisie le nom du genre",
    OperationId = "PostGenre")]
        [SwaggerResponse(200, "genre créer avec succès")]
        [SwaggerResponse(400, "Demande invalide")]
        public IActionResult PostGenre([FromQuery] GenreDTO genreDTO)
        {
            // la saisie 
            var genre = new Genre()
            {
                Nom = genreDTO.Nom
            };
            // ajoute la genre créer 
            _dbLivre.Genre.Add(genre);
            // la sauvegarde 
            _dbLivre.SaveChanges();
            // affiche 
            return Ok(genre);
        }
        // montrer toute les genre
        [HttpGet]
        [SwaggerOperation(
    Summary = "Montre tout les genre",
    Description = "Ici seras montrer tout le sgenre par ordre d'id",
    OperationId = "GetallGenre")]
        [SwaggerResponse(200, "Genre montrer avec succe")]
        [SwaggerResponse(400, "Demande invalide")]
        public IActionResult GetAllGenre()
        {
            //Selectionne toute les genre
            var allGenre = _dbLivre.Genre.ToList();
            // les affiche 
            return Ok(allGenre);
        }
        // le chemin a taper 
        [HttpGet("api/GetGenre")]
        [SwaggerOperation(
    Summary = "montre les genre demander",
    Description = "Ici seras filtrer par ID et par Nom",
    OperationId = "GetGenre")]
        [SwaggerResponse(200, "Voici la recherche effectué selon les critère demander")]
        [SwaggerResponse(400, "Demande invalide")]
        public IActionResult GetGenre([FromQuery] Genre recherche)
        {
            var genre = _dbLivre.Categorie.AsQueryable();
            genre = FiltreRecherche.AppliquerFiltres(genre, recherche);
            // permet de dire que si les critère demander ne mene a rien 
            if (genre.ToList().Count == 0)
            {
                // Message d'erreure 
                return NotFound("Aucun de vos critère na était trouver !");
            }
            // si identifient trouver retourne la recherche 
            return Ok(genre);
        }
        //Delete genre
        [HttpDelete("{id}")]
        [SwaggerOperation(
    Summary = "Suprime un Genre",
    Description = "Ici seras la supression d'un genre selectionner le genre a suprimer avec son ID",
    OperationId = "DeleteGenre")]
        [SwaggerResponse(200, "Genre suprimer avec succes")]
        [SwaggerResponse(400, "Demande invalide")]
        public IActionResult DeleteGenre(int id)
        {
            // cherche si tu le genre exist
            var genre = _dbLivre.Genre.Find(id);
            //Si le genre n'existe pas retourn RIEN 
            if (genre == null)
            {
                //Message d'erreure
                return NotFound("l'id n'est pas trouver !");
            }
            // Sinon Suprime le membre de la DB
            _dbLivre.Genre.Remove(genre);
            // Sauvegarde les changement
            _dbLivre.SaveChanges();
            // retourn rien car le categorie a était surpimer
            return NoContent();
        }
        //Modifier une genre
        [HttpPut("{id}")]
        [SwaggerOperation(
    Summary = "Modifier un Genre",
    Description = "Ici vous pouvez modifier un Genre selectionner",
    OperationId = "PutGenre")]
        [SwaggerResponse(200, "Genre modifier avec Succes")]
        [SwaggerResponse(400, "Demande invalide")]
        public IActionResult UpdateCategorie( [FromQuery]int id, [FromQuery] GenreDTO genreDTO)
        {
            // Cherche L'id demander
            var genre = _dbLivre.Genre.Find(id);
            // si l'id demander n'est pas trouver
            if (genre == null)
            {
                //retourn Notfound
                return NotFound("l'id n'est pas trouver !");
            }
            // se qui est modifiafle 
            genre.Nom = genreDTO.Nom;
            // la sauvegarde 
            _dbLivre.SaveChanges();
            // affichage
            return Ok(genreDTO);
        }
    }
}
