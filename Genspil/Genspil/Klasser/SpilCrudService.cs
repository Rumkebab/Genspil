using Genspil.Data;

namespace Genspil.Klasser
{
    public static class SpilCrudService
    {
        // Opretter et nyt spil og gemmer det med det samme
        public static Spil? OpretNytSpil(string filsti, List<Spil> spilListe)
        {
            int maxGenre = Enum.GetValues(typeof(Genre)).Length;
            int maxStand = Enum.GetValues(typeof(Stand)).Length;

            Console.Clear();
            Console.WriteLine("==================================");
            Console.WriteLine("         OPRET NYT SPIL           ");
            Console.WriteLine("==================================");
            Console.WriteLine("     Tast A for at afbryde        ");
            Console.WriteLine("==================================");

            Console.Write("Indtast titel: ");
            string titel = (Console.ReadLine() ?? "").Trim();
            if (titel.ToUpper() == "A") return null;

            Console.WriteLine("Vælg genre:");
            foreach (var genre in Enum.GetValues(typeof(Genre)))
            {
                int nummer = (int)genre + 1;
                Console.WriteLine($"{nummer} = {genre}");
            }
            Console.Write("> ");

            string genreInput = (Console.ReadLine() ?? "").Trim();
            if (genreInput.ToUpper() == "A") return null;

            if (!int.TryParse(genreInput, out int genreValg) || genreValg < 1 || genreValg > maxGenre)
            {
                Console.WriteLine("Ugyldigt genrevalg.");
                ConsoleHelper.Pause();
                return null;
            }

            Genre valgtGenre = (Genre)(genreValg - 1);

            Console.Write("Indtast antal spillere: ");
            string antalInput = (Console.ReadLine() ?? "").Trim();
            if (antalInput.ToUpper() == "A") return null;

            Console.WriteLine("Vælg stand:");
            foreach (var stand in Enum.GetValues(typeof(Stand)))
            {
                int nummer = (int)stand + 1;
                Console.WriteLine($"{nummer} = {stand}");
            }
            Console.Write("> ");

            string standInput = (Console.ReadLine() ?? "").Trim();
            if (standInput.ToUpper() == "A") return null;

            if (!int.TryParse(standInput, out int standValg) || standValg < 1 || standValg > maxStand)
            {
                Console.WriteLine("Ugyldigt standvalg.");
                ConsoleHelper.Pause();
                return null;
            }

            Stand valgtStand = (Stand)(standValg - 1);

            Console.Write("Indtast pris: ");
            string prisInput = (Console.ReadLine() ?? "").Trim();
            if (prisInput.ToUpper() == "A") return null;

            if (!int.TryParse(prisInput, out int pris))
            {
                Console.WriteLine("Ugyldig pris.");
                ConsoleHelper.Pause();
                return null;
            }

            Spil nytSpil = new Spil(titel, valgtGenre, valgtStand, pris, antalInput);
            spilListe.Add(nytSpil);
            SpilDataHandler.GemTilFil(filsti, spilListe);

            return nytSpil;
        }

        // Sletter et spil ud fra ID
        public static void SletSpil(string filsti, List<Spil> spilListe)
        {
            if (spilListe.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("=== Slet spil ===");
                Console.WriteLine("Ingen spil at slette.");
                ConsoleHelper.VentPåA();
                return;
            }

            char sortering = 'O';

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Slet spil ===");

                List<Spil> sorteretListe = SpilVisningService.HentSorteretListe(spilListe, sortering);
                SpilVisningService.PrintSpilTabel(sorteretListe);

                Console.WriteLine("Sortér efter: [N]avn | [G]enre | [S]tand | [P]ris | [O]prettelsesdato | [A]fbryd");
                Console.Write("Indtast ID på spil der skal slettes: ");

                string input = (Console.ReadLine() ?? "").Trim();

                if (input.ToUpper() == "A")
                    return;

                if (input.Length == 1 && "NGSPO".Contains(char.ToUpper(input[0])))
                {
                    sortering = char.ToUpper(input[0]);
                    continue;
                }

                if (int.TryParse(input, out int id))
                {
                    Spil? spilDerSkalSlettes = spilListe.Find(s => s.Id == id);

                    if (spilDerSkalSlettes != null)
                    {
                        spilListe.Remove(spilDerSkalSlettes);
                        SpilDataHandler.GemTilFil(filsti, spilListe);
                        Console.WriteLine("Spillet er slettet og gemt.");
                    }
                    else
                    {
                        Console.WriteLine("Ingen spil fundet med det ID.");
                    }

                    ConsoleHelper.VentPåA();
                    return;
                }

                Console.WriteLine("Du skal skrive et gyldigt ID eller et sorteringsbogstav.");
                ConsoleHelper.Pause();
            }
        }

        // Redigerer et spil ud fra ID
        public static void RedigerSpil(string filsti, List<Spil> spilListe)
        {
            int maxGenre = Enum.GetValues(typeof(Genre)).Length;
            int maxStand = Enum.GetValues(typeof(Stand)).Length;

            if (spilListe.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("=== Rediger spil ===");
                Console.WriteLine("Ingen spil at redigere.");
                ConsoleHelper.VentPåA();
                return;
            }

            char sortering = 'O';

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Rediger spil ===");

                List<Spil> sorteretListe = SpilVisningService.HentSorteretListe(spilListe, sortering);
                SpilVisningService.PrintSpilTabel(sorteretListe);

                Console.WriteLine("Sortér efter: [N]avn | [G]enre | [S]tand | [P]ris | [O]prettelsesdato | [A]fbryd");
                Console.Write("Indtast ID på spil der skal redigeres: ");

                string input = (Console.ReadLine() ?? "").Trim();

                if (input.ToUpper() == "A")
                    return;

                if (input.Length == 1 && "NGSPO".Contains(char.ToUpper(input[0])))
                {
                    sortering = char.ToUpper(input[0]);
                    continue;
                }

                if (!int.TryParse(input, out int id))
                {
                    Console.WriteLine("Du skal skrive et gyldigt ID eller et sorteringsbogstav.");
                    ConsoleHelper.Pause();
                    continue;
                }

                Spil? valgtSpil = spilListe.Find(s => s.Id == id);

                if (valgtSpil == null)
                {
                    Console.WriteLine("Ingen spil fundet med det ID.");
                    ConsoleHelper.VentPåA();
                    return;
                }

                Console.WriteLine();
                Console.WriteLine("Tryk Enter hvis du vil beholde den nuværende værdi.");
                Console.WriteLine("Tast A for at afbryde.");
                Console.WriteLine("==================================");

                Console.Write($"Ny titel ({valgtSpil.Titel}): ");
                string nyTitel = (Console.ReadLine() ?? "").Trim();
                if (nyTitel.ToUpper() == "A") return;
                if (!string.IsNullOrWhiteSpace(nyTitel))
                    valgtSpil.Titel = nyTitel;

                Console.WriteLine($"\nNuværende genre: {valgtSpil.Genre}");
                foreach (var genre in Enum.GetValues(typeof(Genre)))
                {
                    int nummer = (int)genre + 1;
                    Console.WriteLine($"{nummer} = {genre}");
                }
                Console.Write("Vælg ny genre (tryk Enter for at beholde): ");
                string genreInput = (Console.ReadLine() ?? "").Trim();

                if (genreInput.ToUpper() == "A") return;

                if (!string.IsNullOrWhiteSpace(genreInput) &&
                    int.TryParse(genreInput, out int genreValg) &&
                    genreValg >= 1 && genreValg <= maxGenre)
                {
                    valgtSpil.Genre = (Genre)(genreValg - 1);
                }

                Console.WriteLine($"\nNuværende stand: {valgtSpil.Stand}");
                foreach (var stand in Enum.GetValues(typeof(Stand)))
                {
                    int nummer = (int)stand + 1;
                    Console.WriteLine($"{nummer} = {stand}");
                }
                Console.Write("Vælg ny stand (tryk Enter for at beholde): ");
                string standInput = (Console.ReadLine() ?? "").Trim();

                if (standInput.ToUpper() == "A") return;

                if (!string.IsNullOrWhiteSpace(standInput) &&
                    int.TryParse(standInput, out int standValg) &&
                    standValg >= 1 && standValg <= maxStand)
                {
                    valgtSpil.Stand = (Stand)(standValg - 1);
                }

                Console.Write($"\nNyt antal spillere ({valgtSpil.AntalSpillere}): ");
                string antalInput = (Console.ReadLine() ?? "").Trim();
                if (antalInput.ToUpper() == "A") return;
                if (!string.IsNullOrWhiteSpace(antalInput))
                    valgtSpil.AntalSpillere = antalInput;

                Console.Write($"Ny pris ({valgtSpil.Pris} kr): ");
                string prisInput = (Console.ReadLine() ?? "").Trim();
                if (prisInput.ToUpper() == "A") return;

                if (!string.IsNullOrWhiteSpace(prisInput) && int.TryParse(prisInput, out int nyPris))
                    valgtSpil.Pris = nyPris;

                SpilDataHandler.GemTilFil(filsti, spilListe);

                Console.WriteLine("Spillet er blevet opdateret og gemt.");
                ConsoleHelper.VentPåA();
                return;
            }
        }
    }
}