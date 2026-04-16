using Genspil.Klasser;
using Genspil.Data;

namespace Genspil
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Sti til filen med spillene
            string filsti = "Datafiler/spil.txt";


            // Læser spillene fra filen ved opstart
            List<Spil> spilListe = SpilDataHandler.LæsFraFil(filsti);

            // Starter hovedmenuen
            MenuService.Hovedmenu(filsti, spilListe);
        }
    }
}