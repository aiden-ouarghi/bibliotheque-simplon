using LibrIO.Classes;
using LibrIO.Classes_DTO;
using LibrIO.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using LibrIO.Methode;

namespace LibrIO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategorieController : ControllerBase
    {
        private readonly LibrIODb _dbLivre;
        // Évite la valeur Null 
        public CategorieController(LibrIODb librioDB)
        {
            _dbLivre = librioDB;
        }
        // Créer une catégorie 
        [HttpPost("api/PostCategorie")]
        [SwaggerOperation(
            Summary = "Ajoute une catégorie.",
            Description = "La catégorie permet d'indiquer si c'est un Roman/Manga/Novel, etc. ",
            OperationId = "PostCategorie")]
        [SwaggerResponse(200, "Catégorie ajoutée avec succès !")]
        [SwaggerResponse(400, "Demande invalide.")]
        public IActionResult PostCategorie([FromQuery] CategorieDTO categorieDTO)
        {
            // La saisie 
            var categorie = new Categorie()
            {
                Nom = categorieDTO.Nom
            };
            // Ajoute la catégorie créée
            _dbLivre.Categorie.Add(categorie);
            // La sauvegarde 
            _dbLivre.SaveChanges();
            // L'affiche 
            return Ok(categorie);
        }
        // Affiche toutes les catégories
        [HttpGet]
        [SwaggerOperation(
    Summary = "Affiche toutes les catégories.",
    Description = "Affiche toutes les catégories triées par ordre d'ID.",
    OperationId = "GetAllCategorie")]
        [SwaggerResponse(200, "Les catégories sont affichées avec succès !")]
        [SwaggerResponse(400, "Demande invalide.")]
        public IActionResult GetAllCategorie()
        {
            // Séléctionne toutes les catégories
            var allCategorie = _dbLivre.Categorie.ToList();
            // Les affiche 
            return Ok(allCategorie);
        }
        // Le chemin d'accès obligatoire
        [HttpGet("api/GetCategorie")]
        [SwaggerOperation(
    Summary = "Affiche toutes les catégories.",
    Description = "Affiche les catégories correspondant aux critères demandés.",
    OperationId = "GetCategorie")]
        [SwaggerResponse(200, "Les catégories ont été affichées avec succès !")]
        [SwaggerResponse(400, "Demande invalide.")]
        public IActionResult GetCategiorie([FromQuery] Categorie categories)
        {

            var categorie = _dbLivre.Categorie.AsQueryable();

            categorie = FiltreRecherche.AppliquerFiltres(categorie, categories);
            return Ok(categorie);
        }
        // SUPPRIME une catégorie
        [HttpDelete("{id}")]
        [SwaggerOperation(
    Summary = "Supprime une catégorie.",
    Description = "Permet de supprimer une catégorie par son ID.",
    OperationId = "DeleteCategorie")]
        [SwaggerResponse(200, "Catégorie supprimée avec succès.")]
        [SwaggerResponse(400, "Demande invalide.")]
        public IActionResult DeleteCategorie(int id)
        {
            // Cherche si la catégorie existe
            var categorie = _dbLivre.Categorie.Find(id);
            //  Si la catégorie n'existe pas, retourne Null 
            if (categorie == null)
            {
                // Message d'erreur
                return NotFound("L'ID n'a pas été trouvé.");
            }
            // Sinon supprime la catégorie de la DB
            _dbLivre.Categorie.Remove(categorie);
            // Sauvegarde les changements
            _dbLivre.SaveChanges();
            // Ne retourne rien car la catégorie a été supprimée
            return NoContent();
        }
        // MODIFIE une catégorie
        [HttpPut("{id}")]
        [SwaggerOperation(
    Summary = "Modifie une catégorie.",
    Description = "Permet de modifier une catégorie par son ID et en modifiant son nom.",
    OperationId = "PutCategorie")]
        [SwaggerResponse(200, "Catégorie ajoutée avec succès !")]
        [SwaggerResponse(400, "Demande invalide.")]
        public IActionResult UpdateCategorie(int id,[FromQuery] CategorieDTO categorieDTO)
        {
            // Cherche L'ID demandé
            var categorie = _dbLivre.Categorie.Find(id);
            // Si l'ID demandé n'est pas trouvé
            if (categorie == null)
            {
                // Retourne NotFound
                return NotFound("L'ID n'a pas été trouvé.");
            }
            // Ce qui est modifiable 
            categorie.Nom = categorieDTO.Nom;
            // La sauvegarde 
            _dbLivre.SaveChanges();
            // L'affiche
            return Ok(categorie);
        }
    }
}
