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
          Summary = "Ajoute un emprunt.",
          Description = "Crée un emprunt selon l'ID du membre, l'ID du livre, ainsi que les dates d'emprunt et de retour.",
          OperationId = "PostEmprunt")]
        [SwaggerResponse(200, "Livre emprunté !", typeof(Emprunt))]
        [SwaggerResponse(400, "Demande invalide.")]

        public IActionResult PostEmprunt([FromQuery] EmpruntDTO EmpruntDTO)
        {

            var emprunt = new Emprunt()
            {
                Id_Livre = EmpruntDTO.Id_Livre,
                Id_Membre = EmpruntDTO.Id_Membre,
                DateEmprunt = DateTime.Now,
                DateRetour = DateTime.Now.AddMonths(1) // Ajoute un mois de délai à partir de la date du jour 
            };

            LibrIODb.Emprunt.Add(emprunt);

            // Récupère le livre correspondant au Id_Livre
            var livre = LibrIODb.Livre.SingleOrDefault(e => e.Id == emprunt.Id_Livre);

            // Vérifie que le livre existe
            if (livre != null)
            {
                // Vérifie que le livre est bien disponible
                if (livre.Disponibilite == true)
                {
                    // Passe le statut du livre en : "À nouveau disponible" 
                    livre.Disponibilite = false;
                    emprunt.Encours = true;
                }
                else
                {
                    // Retourne une erreur NotFound
                    return NotFound("Le livre demandé n'existe pas.");
                }
            }

            LibrIODb.SaveChanges();
            return Ok(emprunt);
        }

        // Récupère tous les emprunts en cours
        [HttpGet("api/GetAllEmprunt")]
        [SwaggerOperation(
          Summary = "Récupère tous les emprunts en cours.",
          Description = "Retourne la liste de tous les emprunts en cours.",
          OperationId = "GetAllEmprunt")]
        [SwaggerResponse(200, "Retrouvez tous les emprunts en cours.", typeof(Emprunt))]
        [SwaggerResponse(400, "Demande invalide.")]
        public IActionResult GetAllEmprunt(LibrIODb librIODb)
        {
            var emprunts = LibrIODb.Emprunt;
            var empruntsFiltered = emprunts.Where(e => e.Encours == true);

            return Ok(empruntsFiltered);
        }

        // Récupère tous les emprunts d'un membre
        [HttpGet("api/GetEmpruntsbyMembre")]
        [SwaggerOperation(
          Summary = "Récupère tous les emprunts d'un membre par son ID.",
          Description = "Récupère la liste de tous les emprunts d'un membre par son ID.",
          OperationId = "GetEmpruntsbyMembre")]
        [SwaggerResponse(200, "Liste des emprunts du membre sélectionné.", typeof(Emprunt))]
        [SwaggerResponse(400, "Demande invalide.")]
        public IActionResult GetEmpruntsbyMembre([FromQuery] int id)
        {
            var emprunts = LibrIODb.Emprunt;
            var empruntsFiltered = emprunts.Where(e => e.Id_Membre == id);

            return Ok(empruntsFiltered);
        }

        // Récupère tous les emprunts avec la date de retour demandée
        [HttpGet("api/GetEmpruntsbyRetour")]
        [SwaggerOperation(
          Summary = "Récupère tous les emprunts par date de retour (format YYYY-MM-DD)",
          Description = "Récupère tous les emprunts par date de retour.",
          OperationId = "GetEmpruntsbyRetour")]
        [SwaggerResponse(200, "Liste des emprunts à la date de retour choisie.", typeof(Emprunt))]
        [SwaggerResponse(400, "Demande invalide")]
        public IActionResult GetEmpruntsbyRetour(DateTime date)
        { 
            var emprunts = LibrIODb.Emprunt;
            // .Date force à utiliser le format YYYY-MM-DD
            var empruntsFiltered = emprunts.Where(e => e.DateRetour.Date == date);

            return Ok(empruntsFiltered);
        }

        // Récupère tous les emprunts avec la date d'emprunt demandée
        [HttpGet("api/GetEmpruntsbyEmprunt")]
        [SwaggerOperation(
          Summary = "Récupèrer tous les emprunts par date d'emprunt (Format YYYY-MM-DD)",
          Description = "Récupère tous les emprunts par date d'emprunt.",
          OperationId = "GetEmpruntsbyEmprunt")]
        [SwaggerResponse(200, "Retrouvez tous les emprunts à la date d'emprunt choisie.", typeof(Emprunt))]
        [SwaggerResponse(400, "Demande invalide.")]
        public IActionResult GetEmpruntsbyEmprunt(DateTime date)
        { 
            var emprunts = LibrIODb.Emprunt;
            // .Date force à utiliser le format YYYY-MM-DD
            var empruntsFiltered = emprunts.Where(e => e.DateEmprunt.Date == date);

            return Ok(empruntsFiltered);
        }

        //// Récupère un emprunt par différents critères 
        //// Dont la dateEmprunt, par exemple pour envoyer un rappel 
        //[HttpGet("api/GetEmprunt")]
        //[SwaggerOperation(
        //    Summary = "Récupère les emprunts selon une saisie.",
        //    Description = "Récupère tous les emprunts selon la saisie utilisateur.",
        //    OperationId = "GetEmprunt")]
        //[SwaggerResponse(200, "Liste tous les emprunts correspondant aux critères.")]
        //[SwaggerResponse(400, "Demande invalide.")]
        //public IActionResult GetEmprunt([FromQuery] EmpruntDTOrecherche empruntDTOrecherche)
        //{
        //    var emprunt = LibrIODb.Emprunt.AsQueryable();
        //    emprunt = FiltreRecherche.AppliquerFiltres(EmpruntDTOrecherche, emprunt);

        //    return Ok(emprunt);
        //}

        // MODIFIE un emprunt 
        [HttpPut("api/UpdateEmprunt")]
        [SwaggerOperation(
          Summary = "Modifie un emprunt.",
          Description = "Permet de modifier un emprunt existant.",
          OperationId = "UpdateEmprunt")]
        [SwaggerResponse(200, "L'emprunt a été modifié avec succès !", typeof(Emprunt))]
        [SwaggerResponse(400, "Demande invalide.")]

        // Utilise empruntDTOupdate permet à une autre version de du DTO de modifier la DateRetour, initialement assignée à la création
        public IActionResult UpdateEmprunt([FromQuery] int id, [FromQuery] EmpruntDTOupdate empruntDTOupdate)
        {
            var emprunt = LibrIODb.Emprunt.Find(id);

            if (emprunt == null)
            {
                return NotFound("Aucun emprunt trouvé.");
            }

            emprunt.Id_Livre = empruntDTOupdate.Id_Livre;
            emprunt.Id_Membre = empruntDTOupdate.Id_Membre;
            emprunt.DateRetour = empruntDTOupdate.DateRetour;

            LibrIODb.SaveChanges();
            return Ok(emprunt);
        }

        // Rend un livre en permettant d'archiver l'emprunt mais ne le supprime pas
        [HttpPut("api/RendreEmprunt")]
        [SwaggerOperation(
          Summary = "Rend un emprunt afin de l'utiliser dans l'historique utilisateur.",
          Description = "Rend un emprunt.",
          OperationId = "RendreEmprunt")]
        [SwaggerResponse(200, "Le livre a bien été rendu, l'emprunt a été archivé.", typeof(Emprunt))]
        [SwaggerResponse(400, "Demande invalide.")]

        // Utilise empruntDTOupdate, une autre version de mon DTO qui me permet de modifier la DateRetour, initialement assigné à la création
        public IActionResult RendreEmprunt([FromQuery] int id)
        {
            var emprunt = LibrIODb.Emprunt.Find(id);

            if (emprunt == null)
            {
                return NotFound("Aucun emprunt trouvé.");
            }

            emprunt.Encours = false;

            // Récupère le livre correspondant au Id_Livre
            var livre = LibrIODb.Livre.SingleOrDefault(e => e.Id == emprunt.Id_Livre);

            if (livre != null)
            {
                // Passe le livre en : "À nouveau disponible"  
                livre.Disponibilite = true;
            }

            LibrIODb.SaveChanges();
            return Ok(emprunt);
        }


        // SUPPRIME un Emprunt 
        [HttpDelete("api/DeleteEmprunt")]
        [SwaggerOperation(
          Summary = "Supprime un emprunt.",
          Description = "Permet de supprimer un emprunt.",
          OperationId = "DeleteEmprunt")]
        [SwaggerResponse(200, "Le livre a bien été supprimé.", typeof(Emprunt))]
        [SwaggerResponse(400, "Demande invalide")]
        public IActionResult DeleteEmprunt([FromQuery] int id)
        {
            var emprunt = LibrIODb.Emprunt.Find(id);

            if (emprunt == null)
            {
                return NotFound("Aucun emprunt trouvé.");
            }

            // Récupère le livre correspondant au Id_Livre
            var livre = LibrIODb.Livre.SingleOrDefault(e => e.Id == emprunt.Id_Livre);

            if (livre != null)
            {
                // Passe le livre en : "À nouveau disponible" 
                livre.Disponibilite = true;
            }

            LibrIODb.Remove(emprunt);
            LibrIODb.SaveChanges();
            return NoContent();
        }
    }
}
