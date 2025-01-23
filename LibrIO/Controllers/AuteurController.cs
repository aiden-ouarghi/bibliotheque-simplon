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
    public class AuteurController : ControllerBase
    {
        private readonly LibrIODb _dbLivre;
        // Évite la valeur Null 
        public AuteurController(LibrIODb librioDB)
        {
            _dbLivre = librioDB;
        }
        // Créer un auteur
        [HttpPost]
        // Commentaire Swagger
        [SwaggerOperation(
            Summary = "Ajoute un auteur.",
            Description = "Ajoute un auteur en saisissant un nom, un prénom ou les deux. " +
                    "Dans le cas où le livre n'a pas d'auteur par defaut, l'ID 1 sera sans nom et sans prénom.",
            OperationId = "PostAuteur")]
        [SwaggerResponse(200, "Auteur ajouté avec succès !")]
        [SwaggerResponse(400, "Demande invalide.")]
        public IActionResult PostAuteur([FromQuery] AuteurDTO auteurDTO)
        { 
            // La saisie
            var auteur = new Auteur()
            {
                Nom = auteurDTO.Nom,
                Prenom = auteurDTO.Prenom
            };
            // Ajoute l'auteur crée
            _dbLivre.Auteur.Add(auteur);
            // La sauvegarde 
            _dbLivre.SaveChanges();
            // L'affiche 
            return Ok(auteur);
        }
        // GET all auteur
        [HttpGet]
        [SwaggerOperation(
            Summary = "Affiche tous les auteurs.",
            Description = "Affiche tous les auteurs par ordre d'ID.",
            OperationId = "GetAllAuteur")]
        [SwaggerResponse(200, "Les auteurs sont montrés avec succès !")]
        [SwaggerResponse(400, "Demande invalide.")]
        public IActionResult GetAllAuteur()
        {
            // Séléctionne tous les auteurs
            var allAuteur = _dbLivre.Auteur.ToList();
            // Les affiche 
            return Ok(allAuteur);
        }
        // Le chemin d'accès obligatoire
        [HttpGet("api/GetAuteur")]
        [SwaggerOperation(
            Summary = "Affiche les auteurs demandés.",
            Description = "Affiche les auteurs selon les critères demandés. Aucune obligation de remplir tous les critères.",
            OperationId = "GetAuteur")]
        [SwaggerResponse(200, "Auteur affiché avec succès !")]
        [SwaggerResponse(400, "Demande invalide.")]
        public IActionResult GetAuteur([FromQuery] Auteur auteurs)
        {
            var auteur = _dbLivre.Auteur.AsQueryable();
            // Fait toutes les recherches :D 
            auteur = FiltreRecherche.AppliquerFiltres(auteur, auteurs);
            // Si la recherche est Null 
            if (auteur.ToList().Count == 0)
            {
                // Message d'erreur 
                return NotFound("Aucun de vos critères n'a été trouvé.");
            }
            // Si l'ID est trouvé, il retourne la recherche 
            return Ok(auteur);
        }
        // SUPPRIME un auteur
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Supprime un auteur.",
            Description = "Supprime un auteur à partir de son ID.",
            OperationId = "DeleteAuteur")]
        [SwaggerResponse(200, "Auteur supprimé avec succès.")]
        [SwaggerResponse(400, "Demande invalide.")]
        public IActionResult DeleteAuteur(int id)
        {
            // Cherche si l'ID Auteur existe
            var auteur = _dbLivre.Auteur.Find(id);
            // Si l'auteur n'existe pas, il retourne Null 
            if (auteur == null)
            {
                // Message d'erreur
                return NotFound("L'ID n'a pas été trouvé !");
            }
            // Sinon supprime l'auteur de la DB
            _dbLivre.Auteur.Remove(auteur);
            // Sauvegarde les changements
            _dbLivre.SaveChanges();
            // Ne retourne rien car l'auteur a été supprimé
            return NoContent();
        }
        // MODIFIE un auteur
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Modifie un auteur.",
            Description = "Permet de modifier le nom et/ou le prénom d'un auteur par son ID.",
            OperationId = "PutAuteur")]
        [SwaggerResponse(200, "Auteur modifié avec succès !")]
        [SwaggerResponse(400, "Demande invalide.")]
        public IActionResult UpdateAuteur(int id, [FromQuery] AuteurDTO auteurDTO)
        {
            // Cherche L'ID demandé
            var auteur = _dbLivre.Auteur.Find(id);
            // Si l'ID demandé n'est pas trouvé
            if (auteur == null)
            {
                // Retourne NotFound
                return NotFound("l'ID n'a pas été trouvé.");
            }
            // Ce qui est modifiable
            auteur.Nom = auteurDTO.Nom;
            auteur.Prenom = auteurDTO.Prenom;
            // La sauvegarde 
            _dbLivre.SaveChanges();
            // L'affiche
            return Ok(auteur);
        }
    }
}
