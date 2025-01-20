using LibrIO.Classes;
using LibrIO.Classes_DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LibrIO.Data;

namespace LibrIO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeController : ControllerBase
    {
        private readonly LibrIODb _dbLivre;

        // Éviter la valeur Null 
        public EmployeController(LibrIODb librioDB)
        {
            _dbLivre = librioDB;
        }

        // Créer un employé
        [HttpPost]
        public IActionResult PostEmploye(EmployeDTO employeDTO)
        {
            // ------ INFORMATIONS DEMANDÉES ------ //
            var employeEntite = new Employe()
            {
                Prenom = employeDTO.Prenom,
                Nom = employeDTO.Nom,
                Mail = employeDTO.Mail
            };

            // Ajout à la DB
            _dbLivre.Employe.Add(employeEntite);

            // Sauvegarde
            _dbLivre.SaveChanges();

            // Affichage
            return Ok(employeEntite);
        }

        // Affiche tous les employés
        [HttpGet]
        public IActionResult GetAllEmployes()
        {
            // Sélectionne tous les employés
            var allEmploye = _dbLivre.Employe.ToList();

            if (allEmploye == null)
            {
                return NotFound("Aucun employé n'a été enregistré");
            }

            // Les affichent
            return Ok(allEmploye);
        }

        // Ici il faudra le renommer au bon vouloir de chacun 
        // Pourquoi ici j'utilise Employe et non EmployeDTO ? Car Le DTO ne contient pas d'ID

        [HttpGet]
        // Trouve un employé ou plusieurs selon la recherche
        public IActionResult GetEmploye([FromQuery] Employe Recherche)
        {
            // AsQueryable sert à définir précisément les données sorties de la Base de Données 
            // Elle permet de faire une recherche comme on le souhaite 
            var employe = _dbLivre.Employe.AsQueryable();

            if (!string.IsNullOrEmpty(Recherche.Prenom))
            {
                // Trim Supprime les espaces, ToLower la met en minuscule Et Contains vérifie si Identification contient la valeur demandée
                employe = employe.Where(employePrenom => employePrenom.Prenom.Trim().ToLower().Contains(Recherche.Prenom.Trim().ToLower()));
            }
            if (!string.IsNullOrEmpty(Recherche.Nom))
            {
                employe = employe.Where(employeNom => employeNom.Nom.Trim().ToLower().Contains(Recherche.Nom.Trim().ToLower()));
            }
            if (!string.IsNullOrEmpty(Recherche.Mail))
            {
                employe = employe.Where(employeMail => employeMail.Mail.Trim().ToLower().Contains(Recherche.Mail.Trim().ToLower()));
            }

            // La condition si Id est supérieur à 0 
            if (Recherche.Id > 0)
            {
                // Cherche dans Employe
                employe = employe.Where(employeId => employeId.Id == Recherche.Id);
            }

            // Fait la requête  
            var employeEntite = employe.ToList();

            // Si rien n'est trouvé
            if (!employeEntite.Any())
            {
                // Message d'erreur
                return NotFound("Aucun employé n'a été trouvé avec les critères demandés !");
            }
            else
            {
                // Montre la valeur demandée
                return Ok(employeEntite);
            }
        }

        // DELETE Employe
        [HttpDelete("{id}")]
        public IActionResult DeleteEmploye(int id)
        {
            // Cherche si l'employé existe
            var employe = _dbLivre.Employe.Find(id);

            // Si l'employé n'existe pas, il ne retourne RIEN 
            if (employe == null)
            {
                return NotFound("L'ID n'a pas été trouvé !");
            }

            // Sinon, supprime l'employé de la DB
            _dbLivre.Employe.Remove(employe);

            // Sauvegarde les changements
            _dbLivre.SaveChanges();

            // Ne retourne rien car l'employé a été supprimé
            return NoContent();
        }

        // MODIFIE Employe 
        [HttpPut("{id}")]
        public IActionResult PutEmploye(int id, EmployeDTO employeDTO)
        {
            // Cherche l'ID demandé
            var employe = _dbLivre.Employe.Find(id);

            // Si l'ID demandé n'est pas trouvé
            if (employe == null)
            {
                // Retourne Not Found
                return NotFound("L'ID n'a pas été trouvé !");
            }

            // Ce qui est modifiable 
            employe.Prenom = employeDTO.Prenom;
            employe.Nom = employeDTO.Nom;
            employe.Mail = employeDTO.Mail;

            // La sauvegarde 
            _dbLivre.SaveChanges();

            // Affichage
            return Ok(employe);
        }
    }
}
