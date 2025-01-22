﻿using LibrIO.Classes;
using LibrIO.Classes_DTO;
using LibrIO.Data;
using LibrIO.Methode;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
namespace LibrIO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LivreController : ControllerBase
    {
        private readonly LibrIODb _dbLivre;
        // eviter la valeur Null 
        public LivreController(LibrIODb librioDB)
        {
            _dbLivre = librioDB;
        }
        [HttpPost]
        [SwaggerOperation(
            Summary = "créer un livre et l'ajoute au catalogue",
            Description = "Créer un livre et l'ajoute au catalogue",
            OperationId = "PostLivre")]
        [SwaggerResponse(200, " Le livre était créer et ajouter au caalogue avec succes")]
        [SwaggerResponse(400, "Demande invalide")]
        public IActionResult PostLivre([FromQuery] LivreDTO livreDTO)
        {
            // la saisie 
            var livre = new Livre()
            {
                Titre = livreDTO.Titre,
                ISBN = livreDTO.ISBN,
                Edition = livreDTO.Edition,
                AuteurId = livreDTO.AuteurId,
                CategorieId = livreDTO.CategorieId,
                GenreId = livreDTO.GenreId,
                Disponibilite = true
            };

            // affiche 
            return Ok(livre);
        }
        [HttpGet]
        [SwaggerOperation(
   Summary = "Montre toute les Livre",
   Description = "Ici seras montrer toute les Livre par odre D'id ",
   OperationId = "GetAllLivre")]
        [SwaggerResponse(200, "Les Livre Sont montrer avec succés")]

        [SwaggerResponse(400, "Demande invalide")]
        public IActionResult GetAllLivre()
        {
            //Selectionne toute les Categorie
            var allLivre = _dbLivre.Livre.ToList();
            // les affiche 
            return Ok(allLivre);
        }

        [HttpGet("api/GetLivre")]
        // Récupérer tous les livres disponibless
        [HttpGet("GetAllLivresDispo")]
        [SwaggerOperation(
          Summary = "Récupèrer tous les livres disponibles",
          Description = "Récupère tous les livres disponibles",
          OperationId = "GetAllLivresDispo")]
        [SwaggerResponse(200, "Retrouvez tous les emprunts en cours", typeof(Emprunt))]
        [SwaggerResponse(400, "Requête invalide")]
        public IActionResult GetAllLivresDispo(LibrIODb librIODb)
        {
            var livres = _dbLivre.Livre;
            var livresFiltered = livres.Where(e => e.Disponibilite == true);

            return Ok(livresFiltered);
        }


        [HttpGet("api/GetCategorie")]
        [SwaggerOperation(
            Summary = "Montre les Livre demander",
            Description = "Ici seras montrer les Livre avec les critère demander",
            OperationId = "GetLivre")]
        [SwaggerResponse(200, "Categorie montrer avec succès")]
        [SwaggerResponse(400, "Demande invalide")]
        public IActionResult GetLivre([FromQuery] LivreDToRecherceh livres)                                                                                                                                                                                                                                                                         
        {
            var livre = _dbLivre.Livre.AsQueryable();

            livre = FiltreRecherche.AppliquerFiltres(livre, livres);
            return Ok(livre);
        }
        // Alors enfaite sa delete Livre et Catalogue car si le livre n'existe pas le catalogue nomplue ? logique.
        //Delete Categorie
        [HttpGet("api/GetLivreByID")]
        public IActionResult GetLivreById([FromQuery] int id)
        {
            var livre = _dbLivre.Livre.Find(id);

            
            return Ok(livre);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Suprime une Livre",
            Description = "Permet de suprimer un livre et le l'id catalogue lié au livre",
            OperationId = "DeleteLivre")]
        [SwaggerResponse(200, "Livre supprimé avec succes")]
        [SwaggerResponse(400, "Demande invalide")]
        public IActionResult DeleteLivre(int id)
        {
            // cherche si la Catgeorie exist
            var livre = _dbLivre.Livre.Find(id);
            //Si le categorie n'existe pas retourn RIEN 
            if (livre == null)
            {
                //Message d'erreure
                return NotFound("l'id n'est pas trouver !");
            }
            // Sinon Suprime le livre de la DB
            _dbLivre.Livre.Remove(livre);
            // Sauvegarde les changement
            _dbLivre.SaveChanges();
            // retourn rien car le categorie a était surpimer
            return NoContent();
        }
        [HttpPut("{id}")]
        public IActionResult UpdateLivre(int id, LivreDTO livreDTO)
        {
            // Cherche L'id demander
            var livre = _dbLivre.Livre.Find(id);
            // si l'id demander n'est pas trouver
            if (livre == null)
            {
                //retourn Notfound
                return NotFound("l'id n'est pas trouver !");
            }
            // se qui est modifiafle 
            livre.Titre = livreDTO.Titre;
            livre.ISBN = livreDTO.ISBN;
            livre.Edition = livreDTO.Edition;
            livre.AuteurId = livreDTO.AuteurId;
            livre.GenreId = livreDTO.GenreId;
            livre.CategorieId = livreDTO.CategorieId;
            // la sauvegarde 
            _dbLivre.SaveChanges();
            // affichage
            return Ok(livre);
        }
    }
}
