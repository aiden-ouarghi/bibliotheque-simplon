using LibrIO.Classes;
using LibrIO.Classes_DTO;
using LibrIO.Data;
using LibrIO.Methode;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
namespace LibrIO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LivreController : ControllerBase
    {
        private readonly LibrIODb _dbLivre;
        // Évite la valeur Null 
        public LivreController(LibrIODb librioDB)
        {
            _dbLivre = librioDB;
        }
        [HttpPost]
        [SwaggerOperation(
            Summary = "Crée un livre et l'ajoute au catalogue.",
            Description = "Crée un livre et l'ajoute au catalogue.",
            OperationId = "PostLivre")]
        [SwaggerResponse(200, "Le livre a été créé et ajouté au catalogue avec succès.")]
        [SwaggerResponse(400, "Demande invalide.")]
        public IActionResult PostLivre([FromQuery] LivreDTO livreDTO)
        {
            // La saisie 
            var livre = new Livre()
            {
                Titre = livreDTO.Titre,
                ISBN = livreDTO.ISBN,
                Edition = livreDTO.Edition,
                AuteurId = livreDTO.AuteurId,
                CategorieId = livreDTO.CategorieId,
                GenreId = livreDTO.GenreId,
                Disponibilite = true
            };

            // L'affiche 
            return Ok(livre);
        }
        [HttpGet]
        [SwaggerOperation(
   Summary = "Affiche tous les livres.",
   Description = "Affiche tous les livres triés par ordre d'ID.",
   OperationId = "GetAllLivre")]
        [SwaggerResponse(200, "Les livres sont affichés avec succès.")]

        [SwaggerResponse(400, "Demande invalide.")]
        public IActionResult GetAllLivre()
        {
            // Séléctionne tous les livres
            var allLivre = _dbLivre.Livre.ToList();
            // Les affiche 
            return Ok(allLivre);
        }

        [HttpGet("api/GetLivre")]
        // Récupère tous les livres disponibles
        [HttpGet("GetAllLivresDispo")]
        [SwaggerOperation(
          Summary = "Récupère tous les livres disponibles.",
          Description = "Retourne la liste de tous les livres actuellement disponibles.",
          OperationId = "GetAllLivresDispo")]
        [SwaggerResponse(200, "Voici tous les emprunts actuellement en cours.", typeof(Emprunt))]
        [SwaggerResponse(400, "Demande invalide.")]
        public IActionResult GetAllLivresDispo(LibrIODb librIODb)
        {
            var livres = _dbLivre.Livre;
            var livresFiltered = livres.Where(e => e.Disponibilite == true);

            return Ok(livresFiltered);
        }


        [HttpGet("api/GetCategorie")]
        [SwaggerOperation(
            Summary = "Affiche les livres demandés.",
            Description = "Affiche les livres correspondant aux critères demandés.",
            OperationId = "GetLivre")]
        [SwaggerResponse(200, "Livres affichés avec succès !")]
        [SwaggerResponse(400, "Demande invalide.")]
        public IActionResult GetLivre([FromQuery] LivreDToRecherceh livres)                                                                                                                                                                                                                                                                         
        {
            var livre = _dbLivre.Livre.AsQueryable();

            livre = FiltreRecherche.AppliquerFiltres(livre, livres);
            return Ok(livre);
        }
        // Supprime le livre et le catalogue associé. Si le livre n'existe pas, le catalogue devient obsolète.
        // Supprimer un livre
        [HttpGet("api/GetLivreByID")]
        public IActionResult GetLivreById([FromQuery] int id)
        {
            var livre = _dbLivre.Livre.Find(id);

            
            return Ok(livre);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Supprime un livre",
            Description = "Permet de supprimer un livre ainsi que l'ID du catalogue associé à ce livre.",
            OperationId = "DeleteLivre")]
        [SwaggerResponse(200, "Livre supprimé avec succès !")]
        [SwaggerResponse(400, "Demande invalide.")]
        public IActionResult DeleteLivre(int id)
        {
            // Cherche si le livre existe
            var livre = _dbLivre.Livre.Find(id);
            // Si le livre n'existe pas, retourne Null.
            if (livre == null)
            {
                //Message d'erreure
                return NotFound("ID n'a pas été trouvé !");
            }
            // Sinon supprime le livre de la DB
            _dbLivre.Livre.Remove(livre);
            // Sauvegarde les changements
            _dbLivre.SaveChanges();
            // Ne retourne rien car le livre a été supprimé
            return NoContent();
        }
        [HttpPut("{id}")]
        public IActionResult UpdateLivre(int id, LivreDTO livreDTO)
        {
            // Cherche L'ID demandé
            var livre = _dbLivre.Livre.Find(id);
            // Si l'ID demandé n'est pas trouvé
            if (livre == null)
            {
                // Retourne NotFound
                return NotFound("L'ID n'a pas été trouvé !");
            }
            // Ce qui est modifiable 
            livre.Titre = livreDTO.Titre;
            livre.ISBN = livreDTO.ISBN;
            livre.Edition = livreDTO.Edition;
            livre.AuteurId = livreDTO.AuteurId;
            livre.GenreId = livreDTO.GenreId;
            livre.CategorieId = livreDTO.CategorieId;
            // La sauvegarde 
            _dbLivre.SaveChanges();
            // L'affiche
            return Ok(livre);
        }
    }
}
