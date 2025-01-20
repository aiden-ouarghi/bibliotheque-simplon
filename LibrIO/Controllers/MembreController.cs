using LibrIO.Classes;
using LibrIO.Classes_DTO;
using LibrIO.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibrIO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembreController : ControllerBase
    {
        private readonly LibrIODb _dbLivre;

        // Éviter la valeur Null 
        public MembreController(LibrIODb librioDB)
        {
            _dbLivre = librioDB;
        }

        // Créer un membre
        [HttpPost]
        public IActionResult PostMembre(MembreDTO membreDTO)
        {
            // ------ INFORMATIONS DEMANDÉES ------ //
            var MembreEntite = new Membre()
            {
                Prenom = membreDTO.Prenom,
                Nom = membreDTO.Nom,
                Mail = membreDTO.Mail
            };

            // Ajout à la DB
            _dbLivre.Membre.Add(MembreEntite);

            // Sauvegarde
            _dbLivre.SaveChanges();

            // Affichage
            return Ok(MembreEntite);
        }
        // Affiche tous les membres
        [HttpGet]
        public IActionResult GetAllMembres()
        {
            // Selectionne tout les membre 
            var allMembre = _dbLivre.Membre.ToList();

            if (allMembre == null)
            {
                return NotFound("Aucun membre n'a été enregistré");
            }
            // Les affichent
            return Ok(allMembre);
        }

        // Ici il faudra le renommer au bon vouloir de chacun 
        // Pourquoi ici j'utilise Membre et non MembreDTO ? Car Le DTO ne contient pas d'ID

        [HttpGet]
        // Trouve un membre ou plusieurs selon la recherche
        public IActionResult GetMembre([FromQuery] Membre Recherche)
        {
            // AsQueryable sert à définir precisément les données sorties de la Base de Données 
            // Elle permet de faire une recherche comme on le souhaite 
            var Membre = _dbLivre.Membre.AsQueryable();

            if (!string.IsNullOrEmpty(Recherche.Prenom))
            {
                // Trim Supprime les espaces, ToLower la met en minuscule Et Contains verifie si Identification contient la valeur demandée
                // Même procédé pour la saisie 
                Membre = Membre.Where(membrePrenom => membrePrenom.Prenom.Trim().ToLower().Contains(Recherche.Prenom.Trim().ToLower()));
            }
            if (!string.IsNullOrEmpty(Recherche.Nom))
            {
                Membre = Membre.Where(membreNom => membreNom.Nom.Trim().ToLower().Contains(Recherche.Nom.Trim().ToLower()));
            }
            if (!string.IsNullOrEmpty(Recherche.Mail))
            {
                Membre = Membre.Where(membreMail => membreMail.Mail.Trim().ToLower().Contains(Recherche.Mail.Trim().ToLower()));
            }

            // La condition si Id est supérieur à 0 
            if (Recherche.Id > 0)
            {
                // Cherche dans Membre 
                Membre = Membre.Where(membreId => membreId.Id == Recherche.Id);
            }

            // Fait la requête  
            var MembreEntite = Membre.ToList();

            // si rien n'est trouvé
            if (!MembreEntite.Any())
            {
            // Message d'erreur
                return NotFound("Aucun membre n'a été trouvé avec les critères demandés !");
            }
            else
            {
            // Montre la valeur demandée
                return Ok(MembreEntite);
            }
        }
        // DELETE Membre
        [HttpDelete("{id}")]
        public IActionResult DeleteMembre(int id)
        {
            // Cherche si Membre existe
            var membre = _dbLivre.Membre.Find(id);

            // Si le membre n'existe pas il ne retourne RIEN 
            if (membre == null)
            {
                return NotFound("L'ID n'a pas été trouvé !");
            }
            // Sinon, supprime le membre de la DB
            _dbLivre.Membre.Remove(membre);

            // Sauvegarde les changements
            _dbLivre.SaveChanges();

            // Ne retourne rien car le membre a été supprimé
            return NoContent();
        }


        // MODIFIE Membre 
        [HttpPut("{id}")]
        public IActionResult PutMembre(int id, MembreDTO membreDTO)
        {
            // Cherche L'ID demandé
            var membre = _dbLivre.Membre.Find(id);
            // Si l'ID demandée n'est pas trouvé
            if (membre == null)
            {
                // Retourne Not Found
                return NotFound("L'ID n'a pas été trouvé !");
            }
            // Ce qui est modifiable 
            membre.Prenom = membreDTO.Prenom;
            membre.Nom = membreDTO.Nom;
            membre.Mail = membreDTO.Mail;

            // La sauvegarde 
            _dbLivre.SaveChanges();
            // Affichage
            return Ok(membre);
        }

    }
}
