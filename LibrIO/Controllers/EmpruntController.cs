using LibrIO.Classes;
using LibrIO.Classes_DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using LibrIO.Data;
using LibrIO.Methode;

namespace LibrIO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpruntController : ControllerBase
    {
        private readonly LibrIODb LibrIODb;

        // Ajouter un emprunt 
        public EmpruntController(LibrIODb librIODb)
        {
            LibrIODb = librIODb;
        }

        [HttpPost("api/PostEmprunt")]
        [SwaggerOperation(
          Summary = "Ajouter un emprunt",
          Description = "Crée un emprunt, avec son id membre, son id livre et les dates d'emprunt et de retour",
          OperationId = "PostEmprunt")]
        [SwaggerResponse(200, "Livre emprunté !", typeof(Emprunt))]
        [SwaggerResponse(400, "Demande invalide")]

        public IActionResult PostEmprunt([FromQuery] EmpruntDTO EmpruntDTO)
        {
            var emprunt = new Emprunt()
            {
                Id_Livre = EmpruntDTO.Id_Livre,
                Id_Membre = EmpruntDTO.Id_Membre,
                DateEmprunt = DateTime.Now,
                DateRetour = DateTime.Now.AddMonths(1) // J'ajoute un mois de délai à partir de la date du jour 
            };

            LibrIODb.Emprunt.Add(emprunt);
            LibrIODb.SaveChanges();
            return Ok(emprunt);
        }


        // Récupérer tous les emprunts
        [HttpGet("api/GetAllEmprunt")]
        [SwaggerOperation(
          Summary = "Récupèrer tous les emprunts",
          Description = "Récupère tous les emprunts en cours",
          OperationId = "GetAllEmprunt")]
        [SwaggerResponse(200, "Retrouvez tous les emprunts en cours", typeof(Emprunt))]
        [SwaggerResponse(400, "Requête invalide")]
        public IActionResult GetAllEmprunt(LibrIODb librIODb)
        {
            return Ok(LibrIODb.Emprunt.ToList());
        }

        // Récupérer un emprunt by différents critères 
        // dont la dateEmprunt, par exemple pour envoyer un rappel 
        [HttpGet("api/GetEmprunt")]
        [SwaggerOperation(
            Summary = "Récupèrer les emprunts selon une saisie",
            Description = "Récupère tous les emprunts selon la saisie utilisateur",
            OperationId = "GetEmprunt")]
        [SwaggerResponse(200, "Retrouvez tous les emprunts correspondant à vos critères")]
        [SwaggerResponse(400, "Rêquête invalide")]
        public IActionResult GetEmprunt([FromQuery] Emprunt emprunts)
        {
            var emprunt = LibrIODb.Emprunt.AsQueryable();
            emprunt = FiltreRecherche.AppliquerFiltres(emprunt, emprunts);

            return Ok(emprunt);
        }

        // Modifier un emprunt 
        [HttpPut("api/UpdateEmprunt")]
        [SwaggerOperation(
          Summary = "Modifie un emprunt",
          Description = "Modifie un emprunt",
          OperationId = "UpdateEmprunt")]
        [SwaggerResponse(200, "Le livre a bien été modifié", typeof(Emprunt))]
        [SwaggerResponse(400, "Rêquête invalide")]

        // J'utilise empruntDTOupdate, une autre version de mon DTO qui me permet de modifier la DateRetour, autrement assignée à la création
        public IActionResult UpdateEmprunt([FromQuery] int id, [FromQuery] EmpruntDTOupdate empruntDTOupdate)
        {
            var emprunt = LibrIODb.Emprunt.Find(id);

            if (emprunt == null)
            {
                return NotFound("Pas d'emprunt trouvé !");
            }

            emprunt.Id_Livre = empruntDTOupdate.Id_Livre;
            emprunt.Id_Membre = empruntDTOupdate.Id_Membre;
            emprunt.DateRetour = empruntDTOupdate.DateRetour;

            LibrIODb.SaveChanges();
            return Ok(emprunt);
        }


        // Supprimer un emprunt 
        [HttpDelete("api/DeleteEmprunt")]
        [SwaggerOperation(
          Summary = "Supprimer un emprunt",
          Description = "Supprime un emprunt",
          OperationId = "DeleteEmprunt")]
        [SwaggerResponse(200, "Le livre a bien été supprimé", typeof(Emprunt))]
        [SwaggerResponse(400, "Rêquête invalide")]
        public IActionResult DeleteEmprunt([FromQuery] int id)
        {
            var emprunt = LibrIODb.Emprunt.Find(id);

            if (emprunt == null)
            {
                return NotFound("Pas d'emprunt trouvé !");
            }

            LibrIODb.Remove(emprunt);
            LibrIODb.SaveChanges();
            return NoContent();
        }
    }
}
