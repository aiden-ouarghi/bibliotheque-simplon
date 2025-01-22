using System.Text.Json;
using LibrIO;
using LibrIO.Classes;
using static DbInitializer;
using LibrIO.Data;


public static class DbInitializer
{

    public class LivresJson
    {
        public string? Titre { get; set; }
        public string? ISBN { get; set; }
        public string? Edition { get; set; }
        public int? AuteurId { get; set; }
        public int? GenreId { get; set; }
        public int? CategorieId { get; set; }
        public bool? Disponibilite { get; set; }
    }

    public class CategoriesJson
    {
        public string? Nom { get; set; }
    }

    public class GenresJson
    {
        public string? Nom { get; set; }
    }

    public class AuteursJson
    {
        public string? Nom { get; set; }
        public string? Prenom { get; set; }
    }

    public class MembresJson
    {
        public string? Nom { get; set; }
        public string? Prenom { get; set; }
        public string? Mail { get; set; }
    }

    public class EmpruntsJson
    {
        public DateTime DateEmprunt { get; set; }
        public DateTime DateRetour { get; set; }
        public int Id_Livre { get; set; }
        public int Id_Membre { get; set; }
        public bool Encours { get; set; }

    }

    public static void Seed(LibrIODb context)
    {
        // Chemin vers les fichiers JSON
        string livresFilePath = Directory.GetCurrentDirectory() + "\\JsonData\\livres.json";
        string membresFilePath = Directory.GetCurrentDirectory() + "\\JsonData\\membres.json";
        string empruntsFilePath = Directory.GetCurrentDirectory() + "\\JsonData\\emprunts.json";
        string categoriesFilePath = Directory.GetCurrentDirectory() + "\\JsonData\\categories.json";
        string genresFilePath = Directory.GetCurrentDirectory() + "\\JsonData\\genres.json";
        string auteursFilePath = Directory.GetCurrentDirectory() + "\\JsonData\\auteurs.json";

        // Charger les catégories
        if (!context.Categorie.Any())
        {
            var categoriesJson = File.ReadAllText(categoriesFilePath);
            var categories = JsonSerializer.Deserialize<List<CategoriesJson>>(categoriesJson);
            if (categories != null)
            {
                foreach (var categorieJson in categories)
                {
                    var categorie = new Categorie { Nom = categorieJson.Nom };
                    context.Categorie.Add(categorie);
                }
            }
            context.SaveChanges();
        }

        // Charger les genres
        if (!context.Genre.Any())
        {
            var genresJson = File.ReadAllText(genresFilePath);
            var genres = JsonSerializer.Deserialize<List<GenresJson>>(genresJson);

            if (genres != null)
            {
                foreach (var genreJson in genres)
                {
                    var genre = new Genre { Nom = genreJson.Nom };
                    context.Genre.Add(genre);
                }
            }

            context.SaveChanges();
        }

        // Charger les auteurs
        if (!context.Auteur.Any())
        {
            var auteursJson = File.ReadAllText(auteursFilePath);
            var auteurs = JsonSerializer.Deserialize<List<AuteursJson>>(auteursJson);
            if (auteurs != null)
            {
                foreach (var auteurJson in auteurs)
                {
                    var auteur = new Auteur { Nom = auteurJson.Nom, Prenom = auteurJson.Prenom };
                    context.Auteur.Add(auteur);
                }
            }
            
            context.SaveChanges();
        }

        // Charger les livres
        if (!context.Livre.Any())
        {
            var livresJson = File.ReadAllText(livresFilePath);
            var livres = JsonSerializer.Deserialize<List<LivresJson>>(livresJson);

            if (livres != null)
            {
                foreach (var livreJson in livres)
                {
                    if (context.Auteur.Any(a => a.Id == livreJson.AuteurId) &&
                        context.Genre.Any(g => g.Id == livreJson.GenreId) &&
                        context.Categorie.Any(c => c.Id == livreJson.CategorieId))
                    {
                        var livre = new Livre
                        {
                            Titre = livreJson.Titre,
                            ISBN = livreJson.ISBN,
                            Edition = livreJson.Edition,
                            AuteurId = livreJson.AuteurId,
                            GenreId = livreJson.GenreId,
                            CategorieId = livreJson.CategorieId,
                            Disponibilite = livreJson.Disponibilite
                        };
                        context.Livre.Add(livre);
                    }
                }
            
            }
            context.SaveChanges();
        }

        // Charger les membres
        if (!context.Membre.Any())
        {
            var membresJson = File.ReadAllText(membresFilePath);
            var membres = JsonSerializer.Deserialize<List<MembresJson>>(membresJson);

            if (membres != null)
            {
                foreach (var membreJson in membres)
                {
                    var membre = new Membre
                    {
                        Nom = membreJson.Nom,
                        Prenom = membreJson.Prenom,
                        Mail = membreJson.Mail
                    };
                    context.Membre.Add(membre);
                }
            }
            
            context.SaveChanges();
        }

        // Charger les emprunts
        if (!context.Emprunt.Any())
        {
            var empruntsJson = File.ReadAllText(empruntsFilePath);
            var emprunts = JsonSerializer.Deserialize<List<EmpruntsJson>>(empruntsJson);
            if (emprunts != null)
            {
                foreach (var empruntJson in emprunts)
                {
                    if (context.Livre.Any(l => l.Id == empruntJson.Id_Livre) &&
                        context.Membre.Any(m => m.Id == empruntJson.Id_Membre))
                    {
                        var emprunt = new Emprunt
                        {
                            DateEmprunt = empruntJson.DateEmprunt,
                            DateRetour = empruntJson.DateRetour,
                            Id_Livre = empruntJson.Id_Livre,
                            Id_Membre = empruntJson.Id_Membre,
                            Encours = empruntJson.Encours
                        };
                        context.Emprunt.Add(emprunt);
                    }
                }
            }
            
            context.SaveChanges();
        }
    }
}