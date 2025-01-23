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
        // sa m'evite la valeur Null donc ouaip je garde..
        public MembreController(LibrIODb dbcontext)
        {
            _dbLivre = dbcontext;
        }

        // créer un membre
        [HttpPost]
        [SwaggerOperation(
    Summary = "Créer un membre ",
    Description = "entrer les donnée d'un membre Prenom, Nom, Mail",
    OperationId = "PostMmembre")]
        [SwaggerResponse(200, "Membre créer !")]
        [SwaggerResponse(400, "Demande invalide")]
        public IActionResult PostMembre([FromQuery]MembreDTO MembreDTO)
        {
            // information demander //
            var MembreEntite = new Membre()
            {
                Prenom = MembreDTO.Prenom,
                Nom = MembreDTO.Nom,
                Mail = MembreDTO.Mail
            };
            if(MembreEntite == null)
            {
                return NotFound("Veuillez entrer des saisie correct");
            }
            // Ajout a la DB
            _dbLivre.Membre.Add(MembreEntite);
            // sauvegarde
            _dbLivre.SaveChanges();
            // affichage
            return Ok(MembreEntite);
        }
        // Afficher tous les membre
        [HttpGet]
        [SwaggerOperation(
    Summary = "Afficher tout les membre",
    Description = "ici vous pourrez voir tou les membre ",
    OperationId = "PutAuteur")]
        [SwaggerResponse(200, "Auteur Modifier avec succès")]
        [SwaggerResponse(400, "Demande invalide")]
        public IActionResult GetAllMembres()
        {
            // Selectionne tout les membre 
            var allMembre = _dbLivre.Membre.ToList();

            if (allMembre == null)
            {
                return NotFound("Aucun Membre na était enregistrer");
            }
            // les affiche
            return Ok(allMembre);
        }
        // ici faudras le renommer au bon vouloir de chacun 
        // Pourquoi Ici j'utilise Membre ? Et non MembreDTO Car Le DTO ne contient pas D'id 
        [HttpGet("api/get")]
        [SwaggerOperation(
    Summary = "chercher un Membre",
    Description = "Permet de rechercher un membre avec des critere de recherche ",
    OperationId = "GetMembre")]
        [SwaggerResponse(200, "Auteur Modifier avec succès")]
        [SwaggerResponse(400, "Demande invalide")]
        // trouver un membre ou plusiuer selon la recherche
        public IActionResult GetMembre([FromQuery] Membre RechercheMembre)
        {
            //AsQueryable a definir precisément juste sa rend les donnée sortie de base de donnée plus flexible et manipulable dans ce cas ci sa me permet de 
            //faire ma recherche comme je le souhaite 
            var Membre = _dbLivre.Membre.AsQueryable();

            Membre = FiltreRecherche.AppliquerFiltres(Membre, RechercheMembre);
            // la condition si Id est supérieur a 0 
            if (RechercheMembre.Id > 0)
            {
                // cherche dans Membre 
                Membre = Membre.Where(membreId => membreId.Id == RechercheMembre.Id);
            }
            //Fait la requete  
            var MembreEntite = Membre.ToList();
            //si rien n'est trouver 
            if (!MembreEntite.Any())
            {
                // Message d'erreure 
                return NotFound("Aucun membre trouvée avec les critère demander !");
            }
            else
            {
                // Montre la valeur demander
                return Ok(MembreEntite);
            }
        }
        //Delete Membre
        [HttpDelete("{id}")]
        [SwaggerOperation(
    Summary = "Suprimer un membre",
    Description = "Permet de selectionner un membre par son ID et le suprimer",
    OperationId = "DeleteMembre")]
        [SwaggerResponse(200, "Membre suprimer avec succès")]
        [SwaggerResponse(400, "Demande invalide")]
        public IActionResult DeleteMembre(int id)
        {
            // cherche si tu le Membre exist
            var membre = _dbLivre.Membre.Find(id);
            //Si le membre n'existe pas retourn RIEN 
            if (membre == null)
            {
                return NotFound("l'id n'est pas trouver !");
            }
            // Sinon Suprime le membre de la DB
            _dbLivre.Membre.Remove(membre);
            // Sauvegarde les changement
            _dbLivre.SaveChanges();
            // retourn rien car le membre a était surpimer
            return NoContent();
        }
        //// Modifier un Membre 
        [HttpPut("{id}")]
        [SwaggerOperation(
    Summary = "Modifier les information des membre",
    Description = "Permet de saisir un ID et de modifier le membre lié a l'Id",
    OperationId = "UpdateMembre")]
        [SwaggerResponse(200, "Membre Modifier avec succès")]
        [SwaggerResponse(400, "Demande invalide")]
        public IActionResult UpdateMembre(int id, MembreDTO membreDTO)
        {
            // Cherche L'id demander
            var membre = _dbLivre.Membre.Find(id);
            // si l'id demander n'est pas trouver
            if (membre == null)
            {
                //retourn Notfound
                return NotFound("l'id n'est pas trouver !");
            }
            // se qui est modifiafle 
            membre.Prenom = membreDTO.Prenom;
            membre.Nom = membreDTO.Nom;
            membre.Mail = membreDTO.Mail;
            // la sauvegarde 
            _dbLivre.SaveChanges();
            // affichage
            return Ok(membre);
        }
    }
}
