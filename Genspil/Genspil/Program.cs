using Genspil.Data;
using Genspil.Klasser;

namespace Genspil
{
    // Programmets entry point - starter applikationen
    public class Program
    {
        public static void Main(string[] args)
        {
            // Konstruerer stien til datafilen relativt til programmets placering
            // Går tre mapper op (..\..\..\) for at finde Datafiler mappen i projektstrukturen
            string filsti = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Datafiler\spil.txt");

            // Indlæser eksisterende spil fra fil (eller tom liste hvis filen ikke findes)
            List<Spil> spilListe = SpilDataHandler.LæsFraFil(filsti);

            // Starter hovedmenuen som kører indtil brugeren vælger at afslutte
            MenuService.Hovedmenu(filsti, spilListe);
        }
    }
}