using Genspil.Klasser;

namespace Genspil
{
    public static class SøgningService
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

            foreach (Spil spil in spilListe)
            {
                bool matcher = true;

                if (!string.IsNullOrWhiteSpace(titelSøgning))
                    if (!spil.Titel.ToLower().Contains(titelSøgning.ToLower()))
                        matcher = false;

                if (!string.IsNullOrWhiteSpace(genreSøgning))
                    if (!spil.Genre.ToString().ToLower().Contains(genreSøgning.ToLower()))
                        matcher = false;

                if (!string.IsNullOrWhiteSpace(standSøgning))
                    if (!spil.Stand.ToString().ToLower().Contains(standSøgning.ToLower()))
                        matcher = false;

                if (!string.IsNullOrWhiteSpace(prisSøgning))
                    if (!spil.Pris.ToString().Contains(prisSøgning))
                        matcher = false;

                if (!string.IsNullOrWhiteSpace(spillereSøgning))
                {
                    if (int.TryParse(spillereSøgning, out int antalSpillere))
                    {
                        string[] parts = spil.AntalSpillere.Split('-');

                        if (parts.Length == 1 && int.TryParse(parts[0], out int enkelt))
                        {
                            if (enkelt != antalSpillere)
                                matcher = false;
                        }
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

                if (matcher)
                    fundneSpil.Add(spil);
            }

            Console.WriteLine();

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