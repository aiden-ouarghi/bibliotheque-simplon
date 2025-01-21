using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LibrIO.Methode
{
    public class FiltreRecherche
    {
        // ce code je l'ai reuni en cherchant élément par élément ce commentaire ne restera pas en version finale
        // je ne sais pas si cette version me convient pour le moment peut etre changer pour un like au lieu du startswith 
        // l'idée général et de gerer les saisie 1 par une 1 
        public static IQueryable<T> AppliquerFiltres<T>(IQueryable<T> valeurClasse, object classe)
        {
            var properties = classe.GetType().GetProperties();
            foreach (var property in properties)
            {
                var value = property.GetValue(classe)?.ToString();
                if (!string.IsNullOrEmpty(value))
                {
                    if (property.PropertyType == typeof(string))
                    {
                        valeurClasse = valeurClasse.Where(instanceDelivre => EF.Property<string>(instanceDelivre, property.Name).StartsWith(value.Trim()));
                    }
                    else if (property.PropertyType == typeof(int) && int.TryParse(value, out int intValue))
                    {
                        valeurClasse = valeurClasse.Where(e => EF.Property<int>(e, property.Name) == intValue);
                    }
                    else if (property.PropertyType == typeof(DateTime) && DateTime.TryParse(value, out DateTime datevalue))
                    {
                        valeurClasse = valeurClasse = valeurClasse.Where(e => EF.Property<DateTime>(e, property.Name) == datevalue);
                    }
                }
            }
            return valeurClasse;
        }
    }
}
