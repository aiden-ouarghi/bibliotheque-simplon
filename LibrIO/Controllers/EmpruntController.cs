using LibrIO.Classes;
using LibrIO.Classes_DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using LibrIO.Data;
using LibrIO.Methode;
using System.Text;
using System.Xml;
using static LibrIO.Classes_DTO.EmpruntDTOupdate;
using System.Globalization;

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

            // Récupérer le livre correspondant au Id_Livre
            var livre = LibrIODb.Livre.SingleOrDefault(e => e.Id == emprunt.Id_Livre);

            // Vérifie que le livre existe
            if (livre != null)
            {
                // Vérifie que le livre est bien disponible
                if (livre.Disponibilite == true)
                {
                    // Passer le livre en à nouveau disponible  
                    livre.Disponibilite = false;
                    emprunt.Encours = true;
                }
                else
                {
                    // Retourne une erreur not found
                    return NotFound("Le livre demandé n'existe pas.");
                }
            }

            LibrIODb.SaveChanges();
            return Ok(emprunt);
        }

        // Récupérer tous les emprunts en cours
        [HttpGet("api/GetAllEmprunt")]
        [SwaggerOperation(
          Summary = "Récupèrer tous les emprunts en cours",
          Description = "Récupère tous les emprunts en cours",
          OperationId = "GetAllEmprunt")]
        [SwaggerResponse(200, "Retrouvez tous les emprunts en cours", typeof(Emprunt))]
        [SwaggerResponse(400, "Requête invalide")]
        public IActionResult GetAllEmprunt(LibrIODb librIODb)
        {
            var emprunts = LibrIODb.Emprunt;
            var empruntsFiltered = emprunts.Where(e => e.Encours == true);

            return Ok(empruntsFiltered);
        }

        // Récupérer tous les emprunts d'un membre
        [HttpGet("api/GetEmpruntsbyMembre")]
        [SwaggerOperation(
          Summary = "Récupèrer tous les emprunts d'un membre par son id",
          Description = "Récupère tous les emprunts d'un membre par son id",
          OperationId = "GetEmpruntsbyMembre")]
        [SwaggerResponse(200, "Retrouvez tous les emprunts du membre sélectionné", typeof(Emprunt))]
        [SwaggerResponse(400, "Requête invalide")]
        public IActionResult GetEmpruntsbyMembre([FromQuery] int id)
        {
            var emprunts = LibrIODb.Emprunt;
            var empruntsFiltered = emprunts.Where(e => e.Id_Membre == id);

            return Ok(empruntsFiltered);
        }

        // Récupérer tous les emprunts avec la date retour demandée 
        [HttpGet("api/GetEmpruntsbyRetour")]
        [SwaggerOperation(
          Summary = "Récupèrer tous les emprunts par date de retour (Format YYYY-MM-DD)",
          Description = "Récupère tous les emprunts par date de retour",
          OperationId = "GetEmpruntsbyRetour")]
        [SwaggerResponse(200, "Retrouvez tous les emprunts à la date de retour choisie", typeof(Emprunt))]
        [SwaggerResponse(400, "Requête invalide")]
        public IActionResult GetEmpruntsbyRetour(DateTime date)
        { 
            var emprunts = LibrIODb.Emprunt;
            // .Date force à utiliser le format YYYY-MM-DD
            var empruntsFiltered = emprunts.Where(e => e.DateRetour.Date == date);

            return Ok(empruntsFiltered);
        }

        // Récupérer tous les emprunts avec la date d'emprunt demandée 
        [HttpGet("api/GetEmpruntsbyEmprunt")]
        [SwaggerOperation(
          Summary = "Récupèrer tous les emprunts par date d'emprunt (Format YYYY-MM-DD)",
          Description = "Récupère tous les emprunts par date d'emprunt",
          OperationId = "GetEmpruntsbyEmprunt")]
        [SwaggerResponse(200, "Retrouvez tous les emprunts à la date d'emprunt choisie", typeof(Emprunt))]
        [SwaggerResponse(400, "Requête invalide")]
        public IActionResult GetEmpruntsbyEmprunt(DateTime date)
        { 
            var emprunts = LibrIODb.Emprunt;
            // .Date force à utiliser le format YYYY-MM-DD
            var empruntsFiltered = emprunts.Where(e => e.DateEmprunt.Date == date);

            return Ok(empruntsFiltered);
        }

        //// Récupérer un emprunt by différents critères 
        //// dont la dateEmprunt, par exemple pour envoyer un rappel 
        //[HttpGet("api/GetEmprunt")]
        //[SwaggerOperation(
        //    Summary = "Récupèrer les emprunts selon une saisie",
        //    Description = "Récupère tous les emprunts selon la saisie utilisateur",
        //    OperationId = "GetEmprunt")]
        //[SwaggerResponse(200, "Retrouvez tous les emprunts correspondant à vos critères")]
        //[SwaggerResponse(400, "Rêquête invalide")]
        //public IActionResult GetEmprunt([FromQuery] EmpruntDTOrecherche empruntDTOrecherche)
        //{
        //    var emprunt = LibrIODb.Emprunt.AsQueryable();
        //    emprunt = FiltreRecherche.AppliquerFiltres(EmpruntDTOrecherche, emprunt);

        //    return Ok(emprunt);
        //}

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

        // Rendre un livre
        // Rendre un livre permet d'archiver l'emprunt mais ne le supprime pas
        [HttpPut("api/RendreEmprunt")]
        [SwaggerOperation(
          Summary = "Rendre un emprunt afin de l'utiliser dans l'historique utilisateur",
          Description = "Rend un emprunt",
          OperationId = "RendreEmprunt")]
        [SwaggerResponse(200, "Le livre a bien été rendu, l'emprunt a été archivé", typeof(Emprunt))]
        [SwaggerResponse(400, "Rêquête invalide")]

        // J'utilise empruntDTOupdate, une autre version de mon DTO qui me permet de modifier la DateRetour, autrement assignée à la création
        public IActionResult RendreEmprunt([FromQuery] int id)
        {
            var emprunt = LibrIODb.Emprunt.Find(id);

            if (emprunt == null)
            {
                return NotFound("Pas d'emprunt trouvé !");
            }

            emprunt.Encours = false;

            // Récupérer le livre correspondant au Id_Livre
            var livre = LibrIODb.Livre.SingleOrDefault(e => e.Id == emprunt.Id_Livre);

            if (livre != null)
            {
                // Passer le livre en à nouveau disponible  
                livre.Disponibilite = true;
            }

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

            // Récupérer le livre correspondant au Id_Livre
            var livre = LibrIODb.Livre.SingleOrDefault(e => e.Id == emprunt.Id_Livre);

            if (livre != null)
            {
                // Passer le livre en à nouveau disponible  
                livre.Disponibilite = true;
            }

            LibrIODb.Remove(emprunt);
            LibrIODb.SaveChanges();
            return NoContent();
        }
    }
}
