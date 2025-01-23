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
        // Évite la valeur Null 
        public GenreController(LibrIODb librioDB)
        {
            _dbLivre = librioDB;
        }
        // Créer un genre 
        [HttpPost("api/PostGenre")]
        [SwaggerOperation(
    Summary = "Crée un genre.",
    Description = "Permet de saisir uniquement le nom du genre.",
    OperationId = "PostGenre")]
        [SwaggerResponse(200, "Genre créé avec succès !")]
        [SwaggerResponse(400, "Demande invalide.")]
        public IActionResult PostGenre([FromQuery] GenreDTO genreDTO)
        {
            // La saisie 
            var genre = new Genre()
            {
                Nom = genreDTO.Nom
            };
            // Ajoute le genre crée 
            _dbLivre.Genre.Add(genre);
            // La sauvegarde 
            _dbLivre.SaveChanges();
            // L'affiche 
            return Ok(genre);
        }
        // Affiche tous les genres
        [HttpGet]
        [SwaggerOperation(
    Summary = "Affiche tous les genres.",
    Description = "Affiche tous les genres triés par ordre d'ID.",
    OperationId = "GetallGenre")]
        [SwaggerResponse(200, "Genre affiché avec succès !")]
        [SwaggerResponse(400, "Demande invalide.")]
        public IActionResult GetAllGenre()
        {
            // Séléctionne tous les genres
            var allGenre = _dbLivre.Genre.ToList();
            // Les affiche 
            return Ok(allGenre);
        }
        // Le chemin d'accès obligatoire
        [HttpGet("api/GetGenre")]
        [SwaggerOperation(
    Summary = "Affiche les genres demandés.",
    Description = "Filtre les genres par ID et par nom.",
    OperationId = "GetGenre")]
        [SwaggerResponse(200, "Voici les résultats de la recherche selon les critères demandés.")]
        [SwaggerResponse(400, "Demande invalide.")]
        public IActionResult GetGenre([FromQuery] Genre recherche)
        {
            var genre = _dbLivre.Categorie.AsQueryable();
            genre = FiltreRecherche.AppliquerFiltres(genre, recherche);
            // Vérifie si les critères demandés ne donnent aucun résultat
            if (genre.ToList().Count == 0)
            {
                // Message d'erreur 
                return NotFound("Aucun de vos critères n'a été trouvé.");
            }
            // Si des résultats sont trouvés, retourne la recherche 
            return Ok(genre);
        }
        // SUPPRIME un genre
        [HttpDelete("{id}")]
        [SwaggerOperation(
    Summary = "Supprime un genre.",
    Description = "Permet de supprimer un genre en sélectionnant son ID.",
    OperationId = "DeleteGenre")]
        [SwaggerResponse(200, "Genre supprimé avec succès !")]
        [SwaggerResponse(400, "Demande invalide.")]
        public IActionResult DeleteGenre([FromQuery] int id)
        {
            // Cherche si le genre existe
            var genre = _dbLivre.Genre.Find(id);
            // Si le genre n'existe pas, retourne rien
            if (genre == null)
            {
                // Message d'erreur
                return NotFound("L'ID n'a pas été trouvé.");
            }
            // Sinon supprime le membre de la DB
            _dbLivre.Genre.Remove(genre);
            // Sauvegarde les changements
            _dbLivre.SaveChanges();
            // Ne retourne rien car le genre a été supprimé
            return NoContent();
        }
        //MODIFIE un genre
        [HttpPut("{id}")]
        [SwaggerOperation(
    Summary = "Modifie un genre.",
    Description = "Permet de modifier un genre sélectionné.",
    OperationId = "PutGenre")]
        [SwaggerResponse(200, "Genre modifié avec succès !")]
        [SwaggerResponse(400, "Demande invalide.")]
        public IActionResult UpdateCategorie( [FromQuery]int id, [FromQuery] GenreDTO genreDTO)
        {
            // Cherche L'ID demandé
            var genre = _dbLivre.Genre.Find(id);
            // Si l'ID demandé n'est pas trouvé
            if (genre == null)
            {
                // Retourne NotFound
                return NotFound("L'ID n'a pas été trouvé !");
            }
            // Ce qui est modifiable 
            genre.Nom = genreDTO.Nom;
            // La sauvegarde 
            _dbLivre.SaveChanges();
            // L'affiche
            return Ok(genreDTO);
        }
    }
}
