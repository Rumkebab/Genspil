using Genspil.Klasser; // Giver adgang til model-klassen Spil

namespace Genspil // Angiver at denne klasse hører til i projektets namespace Genspil
{
    public static class SøgningService // Opretter en statisk serviceklasse til alt der handler om søgning
    {
        public static void SøgEfterSpilMenu(List<Spil> spilListe)
        {
            Console.Clear();
            Console.WriteLine("=== Søg efter spil ===");
            SøgEfterSpil(spilListe);
        }

        public static void SøgEfterSpil(List<Spil> spilListe)
        {
            Console.Clear();
            Console.WriteLine("=== Søg efter spil (tryk Enter for at springe over) ===");

            Console.Write("Titel: ");
            string titelSøgning = Console.ReadLine();

            Console.Write("Genre (Strategi/Familie/Kortspil/Quiz/Samarbejde): ");
            string genreSøgning = Console.ReadLine();

            Console.Write("Stand (Ny/God/Slidt): ");
            string standSøgning = Console.ReadLine();

            Console.Write("Pris: ");
            string prisSøgning = Console.ReadLine();

            List<Spil> fundneSpil = new List<Spil>();

            foreach (Spil spil in spilListe)
            {
                bool matcher = true;

                if (!string.IsNullOrWhiteSpace(titelSøgning))
                {
                    if (!spil.Titel.ToLower().Contains(titelSøgning.ToLower()))
                        matcher = false;
                }

                if (!string.IsNullOrWhiteSpace(genreSøgning))
                {
                    if (!spil.Genre.ToString().ToLower().Contains(genreSøgning.ToLower()))
                        matcher = false;
                }

                if (!string.IsNullOrWhiteSpace(standSøgning))
                {
                    if (!spil.Stand.ToString().ToLower().Contains(standSøgning.ToLower()))
                        matcher = false;
                }

                if (!string.IsNullOrWhiteSpace(prisSøgning))
                {
                    if (!spil.Pris.ToString().Contains(prisSøgning))
                        matcher = false;
                }

                if (matcher)
                    fundneSpil.Add(spil);
            }

            Console.WriteLine();

            if (fundneSpil.Count == 0)
                Console.WriteLine("Ingen spil fundet.");
            else
                SpilVisningService.VisAlleSpil(fundneSpil);
            ConsoleHelper.Pause();
        }
    }
}