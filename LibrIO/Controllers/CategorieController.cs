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
        // eviter la valeur Null 
        public CategorieController(LibrIODb librioDB)
        {
            _dbLivre = librioDB;
        }
        // Créer une categorie 
        [HttpPost("api/PostCategorie")]
        [SwaggerOperation(
            Summary = "Ajoute une Categorie",
            Description = "La Categorie sert a dire si c'est un Roman/Manga/Novel ect ",
            OperationId = "PostCategorie")]
        [SwaggerResponse(200, "Categorie ajouté avec succès")]
        [SwaggerResponse(400, "Demande invalide")]
        public IActionResult PostCategorie([FromQuery] CategorieDTO categorieDTO)
        {
            // la saisie 
            var categorie = new Categorie()
            {
                Nom = categorieDTO.Nom
            };
            // ajoute la categorie créer 
            _dbLivre.Categorie.Add(categorie);
            // la sauvegarde 
            _dbLivre.SaveChanges();
            // affiche 
            return Ok(categorie);
        }
        // montrer toute les categorie
        [HttpGet]
        [SwaggerOperation(
    Summary = "Montre toute les Categorie",
    Description = "Ici seras montrer toute les categorie par odre D'id ",
    OperationId = "GetAllCategorie")]
        [SwaggerResponse(200, "Les Categorie Sont montrer avec succés")]
        [SwaggerResponse(400, "Demande invalide")]
        public IActionResult GetAllCategorie()
        {
            //Selectionne toute les Categorie
            var allCategorie = _dbLivre.Categorie.ToList();
            // les affiche 
            return Ok(allCategorie);
        }
        // le chemin a taper Obligatoire 
        [HttpGet("api/GetCategorie")]
        [SwaggerOperation(
    Summary = "Montre les Categorie demander",
    Description = "Ici seras montrer les Categorie avec les critère demander",
    OperationId = "GetCategorie")]
        [SwaggerResponse(200, "Categorie montrer avec succès")]
        [SwaggerResponse(400, "Demande invalide")]
        public IActionResult GetCategiorie([FromQuery] Categorie categories)
        {
            var categorie = _dbLivre.Categorie.AsQueryable();

            categorie = FiltreRecherche.AppliquerFiltres(categorie, categories);
            return Ok(categorie);
        }
        //Delete Categorie
        [HttpDelete("{id}")]
        [SwaggerOperation(
    Summary = "Suprime une Categorie",
    Description = "Permet de suprimer une Categorie Par son ID ",
    OperationId = "DeleteCategorie")]
        [SwaggerResponse(200, "Categorie suprimer avec succès")]
        [SwaggerResponse(400, "Demande invalide")]
        public IActionResult DeleteCategorie(int id)
        {
            // cherche si la Catgeorie exist
            var categorie = _dbLivre.Categorie.Find(id);
            //Si le categorie n'existe pas retourn RIEN 
            if (categorie == null)
            {
                //Message d'erreure
                return NotFound("l'id n'est pas trouver !");
            }
            // Sinon Suprime la Categorie de la DB
            _dbLivre.Categorie.Remove(categorie);
            // Sauvegarde les changement
            _dbLivre.SaveChanges();
            // retourn rien car le categorie a était surpimer
            return NoContent();
        }
        //Modifier une categorie
        [HttpPut("{id}")]
        [SwaggerOperation(
    Summary = "Modifier une Categorie",
    Description = "Vous pourrez modifier une Categorie en saisissant L'id Et en modifiant le Nom",
    OperationId = "PutCategorie")]
        [SwaggerResponse(200, "Auteur ajouté avec succès")]
        [SwaggerResponse(400, "Demande invalide")]
        public IActionResult UpdateCategorie(int id,[FromQuery] CategorieDTO categorieDTO)
        {
            // Cherche L'id demander
            var categorie = _dbLivre.Categorie.Find(id);
            // si l'id demander n'est pas trouver
            if (categorie == null)
            {
                //retourn Notfound
                return NotFound("l'id n'est pas trouver !");
            }
            // se qui est modifiafle 
            categorie.Nom = categorieDTO.Nom;
            // la sauvegarde 
            _dbLivre.SaveChanges();
            // affichage
            return Ok(categorie);
        }
    }
}
