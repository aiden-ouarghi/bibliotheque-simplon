using LibrIO.Classes;
using LibrIO.Classes_DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using LibrIO.Data;

namespace LibrIO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpruntController : ControllerBase
    {
        private readonly LibrIODb LibrIODb;

        public EmpruntController(LibrIODb librIODb)
        {
            LibrIODb = librIODb;
        }

        [HttpPost]
        [SwaggerOperation(
          Summary = "Ajoute un emprunt",
          Description = "Crée un emprunt, avec son id membre, son id livre et les dates ",
          OperationId = "PostEmprunt")]
        [SwaggerResponse(200, "Emprunt ajouté avec succès", typeof(Emprunt))]
        [SwaggerResponse(400, "Demande invalide")]
        public IActionResult PostEmprunt([FromQuery] Emprunt Emprunt)
        {

            var emprunt = new Emprunt()
            {
                DateEmprunt = Emprunt.DateEmprunt,
                DateRetour = Emprunt.DateRetour,
                Id_Livre = Emprunt.Id_Livre,
                Id_Membre = Emprunt.Id_Membre
            };

            LibrIODb.Emprunt.Add(emprunt);
            LibrIODb.SaveChanges();

            return Ok(emprunt);
        }

        [HttpGet]
        [SwaggerOperation(
          Summary = "Récupère un emprunt",
          Description = "Récupère un emprunt, avec son id membre, son id livre et ses dates ",
          OperationId = "GetAllEmprunt")]
        [SwaggerResponse(200, "---", typeof(Emprunt))]
        [SwaggerResponse(400, "---")]
        public IActionResult GetAllEmprunt(LibrIODb librIODb)
        {
               return Ok(LibrIODb.Emprunt.ToList());
        }

        //[HttpGet]
        //[SwaggerOperation(
        //  Summary = "Récupère un emprunt",
        //  Description = "Récupère un emprunt, avec son id membre, son id livre et ses dates ",
        //  OperationId = "GetAllEmprunt")]
        //[SwaggerResponse(200, "---", typeof(Emprunt))]
        //[SwaggerResponse(400, "---")]
        //public IActionResult GetAllEmprunt(LibrIODb librIODb)
        //{
        //    return Ok(LibrIODb.Emprunt.ToList());
        //}

    }
}

