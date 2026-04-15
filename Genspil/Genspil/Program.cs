using Genspil.Klasser;
using Genspil.Data;

namespace Genspil
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Sti til filen med spillene
            string filsti = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Datafiler\spil.txt");
            filsti = Path.GetFullPath(filsti);

            // Læser spillene fra filen ved opstart
            List<Spil> spilListe = SpilDataHandler.LæsFraFil(filsti);

            // Starter hovedmenuen
            MenuService.Hovedmenu(filsti, spilListe);
        }
    }
}