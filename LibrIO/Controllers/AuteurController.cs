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
        // eviter la valeur Null 
        public AuteurController(LibrIODb librioDB)
        {
            _dbLivre = librioDB;
        }
        // Créer un auteur
        [HttpPost]
        // commentaire Swagger
        [SwaggerOperation(
            Summary = "Ajoute un auteur",
            Description = "Pour ajouter un Auteur il faut saisir un prenom ou un nom ou les deux " +
                    "Dans le cas ou le livre n'a pas d'auteur Par defaut L'id 1 seras Sans nom et sans Prenom2.",
            OperationId = "PostAuteur")]
        [SwaggerResponse(200, "Auteur ajouté avec succès")]
        [SwaggerResponse(400, "Demande invalide")]
        public IActionResult PostAuteur([FromQuery] AuteurDTO auteurDTO)
        { 
            //la saisie
            var auteur = new Auteur()
            {
                Nom = auteurDTO.Nom,
                Prenom = auteurDTO.Prenom
            };
            // ajoute la categorie créer 
            _dbLivre.Auteur.Add(auteur);
            // la sauvegarde 
            _dbLivre.SaveChanges();
            // affiche 
            return Ok(auteur);
        }
        // get all Auteur
        [HttpGet]
        [SwaggerOperation(
            Summary = "Montre tout les Auteur",
            Description = "Ici seras montrer tout les Auteur par odre D'id ",
            OperationId = "GetAllAuteur")]
        [SwaggerResponse(200, "Les Auteur Sont montrer avec succés")]
        [SwaggerResponse(400, "Demande invalide")]
        public IActionResult GetAllAuteur()
        {
            //Selectionne tout les Auteur
            var allAuteur = _dbLivre.Auteur.ToList();
            // les affiche 
            return Ok(allAuteur);
        }
        // le chemin a taper Obligatoire 
        [HttpGet("api/GetAuteur")]
        [SwaggerOperation(
            Summary = "Montre les Auteur demander",
            Description = "Ici seras montrer les Auteur avec les critère demander, Aucune obligation de remplir tout les critère",
            OperationId = "GetAuteur")]
        [SwaggerResponse(200, "Auteur montrer avec succès")]
        [SwaggerResponse(400, "Demande invalide")]
        public IActionResult GetAuteur([FromQuery] Auteur auteurs)
        {
            var auteur = _dbLivre.Auteur.AsQueryable();
            //fait toute les recherche :D 
            auteur = FiltreRecherche.AppliquerFiltres(auteur, auteurs);
            //si la recherche est null 
            if (auteur.ToList().Count == 0)
            {
                // Message d'erreure 
                return NotFound("Aucun de vos critère na était trouver !");
            }
            // si identifient trouver retourne la recherche 
            return Ok(auteur);
        }
        // suprimer Auteur 
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Suprime une Auteur",
            Description = "Permet de suprimer une Auteur Par son ID ",
            OperationId = "DeleteAuteur")]
        [SwaggerResponse(200, "Categorie suprimer avec succès")]
        [SwaggerResponse(400, "Demande invalide")]
        public IActionResult DeleteAuteur(int id)
        {
            // cherche si l'Id Auteur exist
            var auteur = _dbLivre.Auteur.Find(id);
            //Si l'Auteur n'existe pas retourn RIEN 
            if (auteur == null)
            {
                //Message d'erreure
                return NotFound("l'id n'est pas trouver !");
            }
            // Sinon Suprime l'Auteur de la DB
            _dbLivre.Auteur.Remove(auteur);
            // Sauvegarde les changement
            _dbLivre.SaveChanges();
            // retourn rien car l'Auteur a était surpimer
            return NoContent();
        }
        //Modifier un Auteur
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Modifier un Auteur",
            Description = " En saisissant L'Id Vous pourrez modifier Le Prenom et Le Nom de l'auteur",
            OperationId = "PutAuteur")]
        [SwaggerResponse(200, "Auteur Modifier avec succès")]
        [SwaggerResponse(400, "Demande invalide")]
        public IActionResult PutAuteur(int id, [FromQuery] AuteurDTO auteurDTO)
        {
            // Cherche L'id demander
            var auteur = _dbLivre.Auteur.Find(id);
            // si l'id demander n'est pas trouver
            if (auteur == null)
            {
                //retourn Notfound
                return NotFound("l'id n'est pas trouver !");
            }
            // se qui est modifiafle 
            auteur.Nom = auteurDTO.Nom;
            auteur.Prenom = auteurDTO.Prenom;
            // la sauvegarde 
            _dbLivre.SaveChanges();
            // affichage
            return Ok(auteur);
        }
    }
}
