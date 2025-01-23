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
    public class EmployeController : ControllerBase
    {
        private readonly LibrIODb _dbLivre;
        // sa m'evite la valeur Null donc ouaip je garde..
        public EmployeController(LibrIODb dbcontext)
        {
            _dbLivre = dbcontext;
        }
        // créer un employe
        [HttpPost]
        [SwaggerOperation(
    Summary = "Ici seras créer un Employe",
    Description = "Ici seras enregistrer un Employe",
    OperationId = "PostEmploye")]
        [SwaggerResponse(200, "Employe créer avec succes !")]
        [SwaggerResponse(400, "Demande invalide")]
        public IActionResult PostEmploye([FromQuery] EmployeDTO employeDTO)
        {
            // information demander //
            var employe = new Employe()
            {
                Prenom = employeDTO.Prenom,
                Nom = employeDTO.Nom,
                Mail = employeDTO.Mail
            };
            // Ajout a la DB
            _dbLivre.Employe.Add(employe);
            // sauvegarde
            _dbLivre.SaveChanges();
            // affichage
            return Ok(employe);
        }
        // Afficher tous les employe
        [HttpGet]
        [SwaggerOperation(
    Summary = "Monstre tous les Employe",
    Description = "Ici seras montrer tous les Employe",
    OperationId = "GetAllEmploye")]
        [SwaggerResponse(200, "les employer sont visible !")]
        [SwaggerResponse(400, "Demande invalide")]
        public IActionResult GetAllEmploye()
        {
            // Selectionne tout les employe
            var allEmploye = _dbLivre.Employe.ToList();

            if (allEmploye == null)
            {
                return NotFound("Aucun employe na était enregistrer");
            }
            // les affiche
            return Ok(allEmploye);
        }
        [HttpGet("api/get")]
        [SwaggerOperation(
    Summary = "Montre les employe avec les critère demander",
    Description = "Ici seras montrer tous les catalogue",
    OperationId = "GetAllCatalogue")]
        [SwaggerResponse(200, "le catalogue et montrer avec succes !")]
        [SwaggerResponse(400, "Demande invalide")]
        // trouver un employe ou plusiuer selon la recherche
        public IActionResult GetEmploye([FromQuery] Employe employes)
        {
            //AsQueryable a definir precisément juste sa rend les donnée sortie de base de donnée plus flexible et manipulable dans ce cas ci sa me permet de 
            //faire ma recherche comme je le souhaite 
            var employe = _dbLivre.Employe.AsQueryable();
            employe = FiltreRecherche.AppliquerFiltres(employe, employes);
            //si rien n'est trouver 
            if (!employe.Any())
            {
                // Message d'erreure 
                return NotFound("Aucun Employe trouvée avec les critère demander !");
            }
            // Montre la valeur demander
            return Ok(employe);
        }
        //Delete employe
        [HttpDelete("{id}")]
        [SwaggerOperation(
    Summary = "Suprimer un Employe",
    Description = "Permet de suprimer un employer sur son ID",
    OperationId = "DeleteEmploye")]
        [SwaggerResponse(200, "Employe Suprimer avec succès")]
        [SwaggerResponse(400, "Demande invalide")]
        public IActionResult DeleteEmploye(int id)
        {
            // cherche si tu le employe exist
            var employe = _dbLivre.Employe.Find(id);
            //Si le employe n'existe pas retourn RIEN 
            if (employe == null)
            {
                return NotFound("l'id n'est pas trouver !");
            }
            // Sinon Suprime le employe de la DB
            _dbLivre.Employe.Remove(employe);
            // Sauvegarde les changement
            _dbLivre.SaveChanges();
            // retourn rien car le employe a était surpimer
            return NoContent();
        }
        // Modifier un employe 
        [HttpPut("{id}")]
        [SwaggerOperation(
    Summary = "Modifier les information d'un Employer",
    Description = "Permet de modifier les information d'un Employer",
    OperationId = "DeleteEmploye")]
        [SwaggerResponse(200, "Employe Suprimer avec succès")]
        [SwaggerResponse(400, "Demande invalide")]
        public IActionResult UpdateEmploye(int id, EmployeDTO employeDTO)
        {
            // Cherche L'id demander
            var employe = _dbLivre.Employe.Find(id);
            // si l'id demander n'est pas trouver
            if (employe == null)
            {
                //retourn Notfound
                return NotFound("l'id n'est pas trouver !");
            }
            // se qui est modifiafle 
            employe.Prenom = employeDTO.Prenom;
            employe.Nom = employeDTO.Nom;
            employe.Mail = employeDTO.Mail;
            // la sauvegarde 
            _dbLivre.SaveChanges();
            // affichage
            return Ok(employe);
        }
    }
}
