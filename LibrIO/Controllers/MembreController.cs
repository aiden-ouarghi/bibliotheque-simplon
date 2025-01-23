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
    public class MembreController : ControllerBase
    {
        private readonly LibrIODb _dbLivre;
        // Évite la valeur Null 
        public MembreController(LibrIODb dbcontext)
        {
            _dbLivre = dbcontext;
        }

        // Créer un membre
        [HttpPost]
        [SwaggerOperation(
    Summary = "Créer un membre",
    Description = "Entre les informations d'un membre : prénom, nom et mail.",
    OperationId = "PostMmembre")]
        [SwaggerResponse(200, "Membre créé avec succès !")]
        [SwaggerResponse(400, "Demande invalide")]
        public IActionResult PostMembre(MembreDTO MembreDTO)
        {
            // Informations demandées
            var MembreEntite = new Membre()
            {
                Prenom = MembreDTO.Prenom,
                Nom = MembreDTO.Nom,
                Mail = MembreDTO.Mail
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
        [SwaggerOperation(
    Summary = "Affiche tous les membres.",
    Description = "Permet d'afficher tous les membres.",

    OperationId = "PutAuteur")]
        [SwaggerResponse(200, "Livre modifié avec succès !")]
        [SwaggerResponse(400, "Demande invalide.")]
        public IActionResult GetAllMembres()
        {
            // Séléctionne tous les membres 
            var allMembre = _dbLivre.Membre.ToList();

            if (allMembre == null)
            {
                return NotFound("Aucun membre n'a été enregistré.");
            }
            // Les affiche
            return Ok(allMembre);
        }
        // Renommer selon les besoins de chacun
        // Utilise empruntDTOupdate, une autre version de mon DTO qui me permet de modifier la DateRetour, autrement assignée à la création
        [HttpGet("api/get")]
        [SwaggerOperation(
    Summary = "Cherche un membre.",
    Description = "Permet de rechercher un membre avec des critères de recherche.",
    OperationId = "GetMembre")]
        [SwaggerResponse(200, "Membre modifié avec succès !")]
        [SwaggerResponse(400, "Demande invalide.")]
        // Trouve un membre ou plusieur selon la recherche
        public IActionResult GetMembre([FromQuery] Membre RechercheMembre)
        {
            // AsQueryable permet de rendre les données plus flexibles et manipulables
            // Dans ce cas, cela permet de modifier la recherche selon les besoins
            var Membre = _dbLivre.Membre.AsQueryable();

            Membre = FiltreRecherche.AppliquerFiltres(Membre, RechercheMembre);
            // La condition si l'ID est supérieur à 0 
            if (RechercheMembre.Id > 0)
            {
                // Cherche dans membre 
                Membre = Membre.Where(membreId => membreId.Id == RechercheMembre.Id);
            }
            // Fait la requête  
            var MembreEntite = Membre.ToList();
            // Si rien n'est trouvé
            if (!MembreEntite.Any())
            {
                // Message d'erreure 
                return NotFound("Aucun membre trouvé avec les critères demandé !");
            }
            else
            {
                // Affiche la valeur demandé
                return Ok(MembreEntite);
            }
        }
        // SUPPRIME un membre
        [HttpDelete("{id}")]
        [SwaggerOperation(
    Summary = "Supprime un membre.",
    Description = "Permet de selectionner un membre par son ID et de le supprimer.",
    OperationId = "DeleteMembre")]
        [SwaggerResponse(200, "Membre supprimé avec succès !")]
        [SwaggerResponse(400, "Demande invalide.")]
        public IActionResult DeleteMembre(int id)
        {
            // Cherche si le membre existe
            var membre = _dbLivre.Membre.Find(id);
            // Si le membre n'existe pas retourne Null 
            if (membre == null)
            {
                return NotFound("ID n'a pas été trouvé !");
            }
            // Sinon Supprime le membre de la DB
            _dbLivre.Membre.Remove(membre);
            // Sauvegarde les changements
            _dbLivre.SaveChanges();
            // Retourne la valeur Null car le membre a été supprimé
            return NoContent();
        }
        // MODOFIE un membre 
        [HttpPut("{id}")]
        [SwaggerOperation(
    Summary = "Modifie les informations des membres.",
    Description = "Permet de saisir un ID et de modifier le membre lié a l'ID.",
    OperationId = "UpdateMembre")]
        [SwaggerResponse(200, "Membre Modifier avec succès")]
        [SwaggerResponse(400, "Demande invalide")]
        public IActionResult UpdateMembre(int id, MembreDTO membreDTO)
        {
            // Cherche l'ID demandé
            var membre = _dbLivre.Membre.Find(id);
            // Si l'ID demandé n'est pas trouvé
            if (membre == null)
            {
                // Retourne NotFound
                return NotFound("L'ID n'a pas été trouvé !");
            }
            // Ce qui est modifiable 
            membre.Prenom = membreDTO.Prenom;
            membre.Nom = membreDTO.Nom;
            membre.Mail = membreDTO.Mail;
            // La sauvegarde 
            _dbLivre.SaveChanges();
            // L'affiche
            return Ok(membre);
        }
    }
}
