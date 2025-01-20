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
    public class CatalogueController : ControllerBase
    {
        private readonly LibrIODb _dbLivre;
        // eviter la valeur Null 
        public CatalogueController(LibrIODb librioDB)
        {
            _dbLivre = librioDB;
        }
        // ici faut rajouter les 2 get et c'est clean.
        [HttpGet]
        [SwaggerOperation(
    Summary = "Montre tout les Catalogue",
    Description = "Ici seras montrer tous les catalogue",
    OperationId = "GetAllCatalogue")]
        [SwaggerResponse(200, "le catalogue et montrer avec succes !")]
        [SwaggerResponse(400, "Demande invalide")]
        public IActionResult GetAllCatalogue()
        {
            //Selectionne tout les Auteur
            var allCatalogue = _dbLivre.Catalogue.ToList();
            // les affiche 
            return Ok(allCatalogue);
        }
    }
}