using Genspil.Klasser;

namespace Genspil
{
    // Håndterer søgefunktionalitet for at finde spil baseret på forskellige kriterier
    public static class SøgningService
    {
        // Entry point for søgning - viser overskrift og starter søgeprocessen
        public static void SøgEfterSpilMenu(List<Spil> spilListe)
        {
            Console.Clear();
            Console.WriteLine("=== Søg efter spil ===");
            SøgEfterSpil(spilListe);
        }

        // Gennemfører søgning med flere kriterier (alle er valgfrie)
        public static void SøgEfterSpil(List<Spil> spilListe)
        {
            Console.Clear();
            Console.WriteLine("=== Søg efter spil (tryk Enter for at springe over) ===");

            // Indsamler søgekriterier fra brugeren
            Console.Write("Titel: ");
            string titelSøgning = (Console.ReadLine() ?? "").Trim();

            Console.Write("Genre (Strategi/Familie/Kortspil/Quiz/Samarbejde): ");
            string genreSøgning = (Console.ReadLine() ?? "").Trim();

            Console.Write("Stand (Ny/Uåbnet/God/Slidt/Reparation): ");
            string standSøgning = (Console.ReadLine() ?? "").Trim();

            Console.Write("Pris: ");
            string prisSøgning = (Console.ReadLine() ?? "").Trim();

            Console.Write("Antal spillere (fx 3): ");
            string spillereSøgning = (Console.ReadLine() ?? "").Trim();

            List<Spil> fundneSpil = new List<Spil>();

            // Filtrer spil baseret på de indtastede kriterier
            foreach (Spil spil in spilListe)
            {
                bool matcher = true;

                // Case-insensitive søgning i titel
                if (!string.IsNullOrWhiteSpace(titelSøgning))
                    if (!spil.Titel.ToLower().Contains(titelSøgning.ToLower()))
                        matcher = false;

                // Søgning i genre
                if (!string.IsNullOrWhiteSpace(genreSøgning))
                    if (!spil.Genre.ToString().ToLower().Contains(genreSøgning.ToLower()))
                        matcher = false;

                // Søgning i stand
                if (!string.IsNullOrWhiteSpace(standSøgning))
                    if (!spil.Stand.ToString().ToLower().Contains(standSøgning.ToLower()))
                        matcher = false;

                // Søgning i pris
                if (!string.IsNullOrWhiteSpace(prisSøgning))
                    if (!spil.Pris.ToString().Contains(prisSøgning))
                        matcher = false;

                // Søgning efter antal spillere - håndterer både enkeltværdier og intervaller (fx "2-4")
                if (!string.IsNullOrWhiteSpace(spillereSøgning))
                {
                    if (int.TryParse(spillereSøgning, out int antalSpillere))
                    {
                        string[] parts = spil.AntalSpillere.Split('-');

                        // Tjekker om det er et fast antal (fx "4")
                        if (parts.Length == 1 && int.TryParse(parts[0], out int enkelt))
                        {
                            if (enkelt != antalSpillere)
                                matcher = false;
                        }
                        // Tjekker om søgte antal ligger indenfor interval (fx "2-4")
                        else if (parts.Length == 2 &&
                                 int.TryParse(parts[0], out int min) &&
                                 int.TryParse(parts[1], out int max))
                        {
                            if (!(min <= antalSpillere && max >= antalSpillere))
                                matcher = false;
                        }
                        else
                        {
                            matcher = false;
                        }
                    }
                    else
                    {
                        matcher = false;
                    }
                }

                // Tilføj spil til resultater hvis alle kriterier matchede
                if (matcher)
                    fundneSpil.Add(spil);
            }

            Console.WriteLine();

            // Viser resultater eller "ingen fundet" besked
            if (fundneSpil.Count == 0)
            {
                Console.WriteLine("Ingen spil fundet.");
                ConsoleHelper.VentPåA();
            }
            else
            {
                Console.WriteLine($"Fandt {fundneSpil.Count} eksemplar(er).");
                ConsoleHelper.Pause();
                SpilVisningService.VisAlleSpil(fundneSpil);
            }
        }
    }
}