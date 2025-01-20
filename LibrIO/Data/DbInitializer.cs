using System.Text.Json;
using LibrIO;
using LibrIO.Classes;
using static DbInitializer;

public static class DbInitializer
{

    public class LivresJson
    {
        public string Auteur { get; set; }
        public string Titre { get; set; }
        public string ISBN { get; set; }
        public string Edition { get; set; }
        public bool StatutEmprunt { get; set; }
    }

    public class MembresJson
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Mail { get; set; }
    }

    public class EmpruntsJson
    {
        public DateTime DateEmprunt { get; set; }
        public DateTime DateRetour { get; set; }
        public int Id_Livre { get; set; }
        public int Id_Membre { get; set; }
    }

    public static void Seed(LibrIODb context)
    {
        // Chemin vers les fichiers JSON
        string livresFilePath = Directory.GetCurrentDirectory() + "\\JsonData\\livres.json";
        string membresFilePath = Directory.GetCurrentDirectory() + "\\JsonData\\membres.json";
        string empruntsFilePath = Directory.GetCurrentDirectory() + "\\JsonData\\emprunts.json";

        // Charger les livres
        if (!context.Livres.Any())
        {
            var livresJson = File.ReadAllText(livresFilePath);
            var livres = JsonSerializer.Deserialize<List<LivresJson>>(livresJson);

            // Pour tous les livres déclarés dans livresJson, on crée un objet Livre
            foreach (var livreJson in livres)
            {
                var livre = new Livre
                {
                    Auteur = livreJson.Auteur,
                    Titre = livreJson.Titre,
                    ISBN = livreJson.ISBN,
                    Edition = livreJson.Edition,
                    StatutEmprunt = livreJson.StatutEmprunt
                };

                // On l'ajoute au contexte + on le sauvegarde
                context.Livres.AddRange(livre);
                context.SaveChanges();
            }
        }

        //Charger les membres
        if (!context.Membres.Any())
        {
            var membresJson = File.ReadAllText(membresFilePath);
            var membres = JsonSerializer.Deserialize<List<MembresJson>>(membresJson);

            // Pour tous les membres déclarés dans membresJson, on crée un objet Membre
            foreach (var membreJson in membres)
            {
                var membre = new Membre
                {
                    Nom = membreJson.Nom,
                    Prenom = membreJson.Prenom,
                    Mail = membreJson.Mail
                };

                // On l'ajoute au contexte + on le sauvegarde
                context.Membres.AddRange(membre);
                context.SaveChanges();
            }
        }

        // Charger les emprunts
        if (!context.Emprunts.Any())
        {
            var empruntsJson = File.ReadAllText(empruntsFilePath);
            var emprunts = JsonSerializer.Deserialize<List<EmpruntsJson>>(empruntsJson);

            // Pour tous les membres déclarés dans membresJson, on crée un objet Membre
            foreach (var empruntJson in emprunts)
            {
                var emprunt = new Emprunt
                {
                    DateEmprunt = empruntJson.DateEmprunt,
                    DateRetour = empruntJson.DateRetour,
                    Id_Livre = empruntJson.Id_Livre,
                    Id_Membre = empruntJson.Id_Membre
                };

                // On l'ajoute au contexte + on le sauvegarde
                context.Emprunts.AddRange(emprunt);
                context.SaveChanges();
            }
        }

    }
}