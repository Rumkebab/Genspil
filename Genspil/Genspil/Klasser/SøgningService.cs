namespace Genspil.Klasser
{
    public static class SøgningService
    {
        // Starter søgningen
        public static void SøgEfterSpilMenu(List<Spil> spilListe)
        {
            Console.Clear();
            Console.WriteLine("=== Søg efter spil ===");
            SøgEfterSpil(spilListe);
        }

        // Søger på titel, genre, stand og pris
        public static void SøgEfterSpil(List<Spil> spilListe)
        {
            Console.Clear();
            Console.WriteLine("=== Søg efter spil (tryk Enter for at springe over) ===");

            Console.Write("Titel: ");
            string titelSøgning = (Console.ReadLine() ?? "").Trim();

            Console.Write("Genre: ");
            string genreSøgning = (Console.ReadLine() ?? "").Trim();

            Console.Write("Stand: ");
            string standSøgning = (Console.ReadLine() ?? "").Trim();

            Console.Write("Pris: ");
            string prisSøgning = (Console.ReadLine() ?? "").Trim();

            List<Spil> fundneSpil = new List<Spil>();

            foreach (Spil spil in spilListe)
            {
                bool matcherTitel = string.IsNullOrWhiteSpace(titelSøgning) ||
                                    spil.Titel.ToLower().Contains(titelSøgning.ToLower());

                bool matcherGenre = string.IsNullOrWhiteSpace(genreSøgning) ||
                                    spil.Genre.ToString().ToLower().Contains(genreSøgning.ToLower());

                bool matcherStand = string.IsNullOrWhiteSpace(standSøgning) ||
                                    spil.Stand.ToString().ToLower().Contains(standSøgning.ToLower());

                bool matcherPris = string.IsNullOrWhiteSpace(prisSøgning) ||
                                   spil.Pris.ToString().Contains(prisSøgning);

                if (matcherTitel && matcherGenre && matcherStand && matcherPris)
                {
                    fundneSpil.Add(spil);
                }
            }

            Console.WriteLine();

            if (fundneSpil.Count == 0)
            {
                Console.WriteLine("Ingen spil fundet.");
                ConsoleHelper.Pause();
            }
            else
            {
                // Viser søgeresultater uden reservationsstyring
                SpilVisningService.VisAlleSpil(fundneSpil);
            }
        }
    }
}