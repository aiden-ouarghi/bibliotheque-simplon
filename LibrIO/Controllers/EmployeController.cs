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
        // Empêche la valeur Null
        public EmployeController(LibrIODb dbcontext)
        {
            _dbLivre = dbcontext;
        }
        // Créer un employé
        [HttpPost]
        [SwaggerOperation(
    Summary = "Crée un employé.",
    Description = "Enregistre un employé dans la base de données.",
    OperationId = "PostEmploye")]
        [SwaggerResponse(200, "Employé créé avec succès !")]
        [SwaggerResponse(400, "Demande invalide.")]
        public IActionResult PostEmploye(EmployeDTO employeDTO)
        {
            // Informations demandées
            var employe = new Employe()
            {
                Prenom = employeDTO.Prenom,
                Nom = employeDTO.Nom,
                Mail = employeDTO.Mail
            };
            // Ajout à la DB
            _dbLivre.Employe.Add(employe);
            // La sauvegarde
            _dbLivre.SaveChanges();
            // L'affiche
            return Ok(employe);
        }
        // Affiche tous les employés
        [HttpGet]
        [SwaggerOperation(
    Summary = "Affiche tous les employés.",
    Description = "Retourne la liste de tous les employés.",
    OperationId = "GetAllEmploye")]
        [SwaggerResponse(200, "Les employés sont affichés avec succès !")]
        [SwaggerResponse(400, "Demande invalide.")]
        public IActionResult GetAllEmploye()
        {
            // Sélectionne tous les employés
            var allEmploye = _dbLivre.Employe.ToList();

            if (allEmploye == null)
            {
                return NotFound("Aucun Employé n'a été enregistré.");
            }
            // Les affiche
            return Ok(allEmploye);
        }
        [HttpGet("api/get")]
        [SwaggerOperation(
    Summary = "Montre les employés avec les critères demandés.",
    Description = "Affiche les employés avec les critères demandés.",
    OperationId = "GetAllEmploye")]
        [SwaggerResponse(200, "Les employés sont affichés avec succès !")]
        [SwaggerResponse(400, "Demande invalide.")]

        // Trouve un employé ou plusieurs selon la recherche
        public IActionResult GetEmploye([FromQuery] Employe employes)
        {
            // AsQueryable permet de rendre les données plus flexibles et manipulables
            // Dans ce cas, cela permet de modifier la recherche selon les besoins
            var employe = _dbLivre.Employe.AsQueryable();
            employe = FiltreRecherche.AppliquerFiltres(employe, employes);
            // Si rien n'est trouvé 
            if (!employe.Any())
            {
                // Message d'erreur
                return NotFound("Aucun employé trouvé avec les critères demandés.");
            }
            // Affiche la valeur demandée
            return Ok(employe);
        }
        // SUPPRIME un employé
        [HttpDelete("{id}")]
        [SwaggerOperation(
    Summary = "Supprime un employé.",
    Description = "Permet de supprimer un employé par son ID.",
    OperationId = "DeleteEmploye")]
        [SwaggerResponse(200, "Employé supprimé avec succès !")]
        [SwaggerResponse(400, "Demande invalide.")]
        public IActionResult DeleteEmploye(int id)
        {
            // Cherche si l'employé existe
            var employe = _dbLivre.Employe.Find(id);
            // Si l'employé n'existe pas, il retourne Null 
            if (employe == null)
            {
                return NotFound("l'ID n'a pas été trouvé.");
            }
            // Sinon supprime l'employé de la DB
            _dbLivre.Employe.Remove(employe);
            // Sauvegarde les changements
            _dbLivre.SaveChanges();
            // Retourne la valeur Null car l'employé a été supprimé
            return NoContent();
        }
        // MODIFIE un employé 
        [HttpPut("{id}")]
        [SwaggerOperation(
    Summary = "Modifie les informations d'un employé.",
    Description = "Permet de modifier les informations d'un employé.",
    OperationId = "DeleteEmploye")]
        [SwaggerResponse(200, "Employé supprimé avec succès !")]
        [SwaggerResponse(400, "Demande invalide.")]
        public IActionResult UpdateEmploye(int id, EmployeDTO employeDTO)
        {
            // Cherche L'ID demandé
            var employe = _dbLivre.Employe.Find(id);
            // Si l'ID demandé n'est pas trouvé
            if (employe == null)
            {
                // Retourne NotFound
                return NotFound("l'ID n'a pas été trouvé !");
            }
            // Ce qui est modifiable 
            employe.Prenom = employeDTO.Prenom;
            employe.Nom = employeDTO.Nom;
            employe.Mail = employeDTO.Mail;
            // La sauvegarde 
            _dbLivre.SaveChanges();
            // L'affiche
            return Ok(employe);
        }
    }
}
