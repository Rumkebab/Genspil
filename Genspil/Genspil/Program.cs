using Genspil.Data;
using Genspil.Klasser;

namespace Genspil
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Sti til tekstfilen med spillene
            string filsti = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Datafiler\spil.txt");

            // Læser spillene ind og starter hovedmenuen
            List<Spil> spilListe = SpilDataHandler.LæsFraFil(filsti);
            MenuService.Hovedmenu(filsti, spilListe);
        }
    }
}