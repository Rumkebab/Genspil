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

            Console.Write("Er dette et ønsket spil/forespørgsel? (J/N): ");
            string requestInput = (Console.ReadLine() ?? "").Trim().ToUpper();
            if (requestInput == "A") return null;

            bool erRequest = requestInput == "J";
            string kontaktperson = "";

            if (erRequest)
            {
                Console.Write("Kontaktperson (navn på den der har forespurgt): ");
                kontaktperson = (Console.ReadLine() ?? "").Trim();
                if (kontaktperson.ToUpper() == "A") return null;
            }

            Spil nytSpil = new Spil(titel, valgtGenre, valgtStand, pris, antalInput, erRequest: erRequest, kontaktperson: kontaktperson);
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

                Console.WriteLine("Sortér efter: [N]avn | [G]enre | [S]tand | [P]ris | [O]prettelsesdato | [L]spillere | [A]fbryd");
                Console.Write("Indtast ID på spil der skal slettes: ");

                string input = (Console.ReadLine() ?? "").Trim();

                if (input.ToUpper() == "A")
                    return;

                if (input.Length == 1 && "NGSPOL".Contains(char.ToUpper(input[0])))
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

                Console.WriteLine("Sortér efter: [N]avn | [G]enre | [S]tand | [P]ris | [O]prettelsesdato | [L]spillere | [A]fbryd");
                Console.Write("Indtast ID på spil der skal redigeres: ");

                string input = (Console.ReadLine() ?? "").Trim();

                if (input.ToUpper() == "A")
                    return;

                if (input.Length == 1 && "NGSPOL".Contains(char.ToUpper(input[0])))
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

                string title = $"=== Rediger {valgtSpil.Titel} ===";
                string newHeader = title + "\nTryk Enter hvis du vil beholde den nuværende værdi.\nTast A for at afbryde.\n" + new string('=', title.Length) + "";

                Console.Clear();
                Console.WriteLine(newHeader);
                Console.Write($"NUVÆRENDE TITEL: {valgtSpil.Titel}\n\nTryk Enter for at beholde titel\nEller skriv ny titel:\n> ");
                string nyTitel = (Console.ReadLine() ?? "").Trim();
                if (nyTitel.ToUpper() == "A") return;
                if (!string.IsNullOrWhiteSpace(nyTitel))
                    valgtSpil.Titel = nyTitel;

                Console.Clear();
                Console.WriteLine(newHeader);
                Console.WriteLine($"Nuværende genre: {valgtSpil.Genre}\n");
                Console.WriteLine("Muligheder:");
                foreach (var genre in Enum.GetValues(typeof(Genre)))
                {
                    int nummer = (int)genre + 1;
                    Console.WriteLine($"{nummer} = {genre}");
                }
                Console.Write("\nTryk Enter for at beholde genre\nEller skriv ny genre:\n> ");
                string genreInput = (Console.ReadLine() ?? "").Trim();
                if (genreInput.ToUpper() == "A") return;

                if (!string.IsNullOrWhiteSpace(genreInput) &&
                    int.TryParse(genreInput, out int genreValg) &&
                    genreValg >= 1 && genreValg <= maxGenre)
                {
                    valgtSpil.Genre = (Genre)(genreValg - 1);
                }

                Console.Clear();
                Console.WriteLine(newHeader);
                Console.WriteLine($"Nuværende stand: {valgtSpil.Stand}\n");
                Console.WriteLine("Muligheder:");
                foreach (var stand in Enum.GetValues(typeof(Stand)))
                {
                    int nummer = (int)stand + 1;
                    Console.WriteLine($"{nummer} = {stand}");
                }
                Console.Write("\nTryk Enter for at beholde stand\nEller skriv ny stand:\n> ");
                string standInput = (Console.ReadLine() ?? "").Trim();
                if (standInput.ToUpper() == "A") return;

                if (!string.IsNullOrWhiteSpace(standInput) &&
                    int.TryParse(standInput, out int standValg) &&
                    standValg >= 1 && standValg <= maxStand)
                {
                    valgtSpil.Stand = (Stand)(standValg - 1);
                }

                Console.Clear();
                Console.WriteLine(newHeader);
                Console.Write($"Antal spillere: {valgtSpil.AntalSpillere}\n\nTryk Enter for at beholde antal\nEller skriv nyt antal:\n> ");
                string antalInput = (Console.ReadLine() ?? "").Trim();
                if (antalInput.ToUpper() == "A") return;
                if (!string.IsNullOrWhiteSpace(antalInput))
                    valgtSpil.AntalSpillere = antalInput;

                Console.Clear();
                Console.WriteLine(newHeader);
                Console.Write($"Pris: {valgtSpil.Pris} kr\n\nTryk Enter for at beholde pris\nEller skriv ny pris:\n> ");
                string prisInput = (Console.ReadLine() ?? "").Trim();
                if (prisInput.ToUpper() == "A") return;
                if (!string.IsNullOrWhiteSpace(prisInput) && int.TryParse(prisInput, out int nyPris))
                    valgtSpil.Pris = nyPris;

                Console.Clear();
                Console.WriteLine(newHeader);
                Console.Write($"Er det et ønsket spil: {(valgtSpil.ErRequest ? "Ja" : "Nej")}\n\nTryk Enter for at beholde\nEller skriv J eller N:\n> ");
                string requestRedigering = (Console.ReadLine() ?? "").Trim().ToUpper();
                if (requestRedigering == "A") return;

                if (requestRedigering == "J")
                    valgtSpil.ErRequest = true;
                else if (requestRedigering == "N")
                {
                    valgtSpil.ErRequest = false;
                    valgtSpil.Kontaktperson = "";
                }

                if (valgtSpil.ErRequest)
                {
                    Console.Clear();
                    Console.WriteLine(newHeader);
                    Console.Write($"Kontaktperson: {valgtSpil.Kontaktperson}\n\nTryk Enter for at beholde\nEller skriv ny kontaktperson:\n> ");
                    string nyKontakt = (Console.ReadLine() ?? "").Trim();
                    if (nyKontakt.ToUpper() == "A") return;
                    if (!string.IsNullOrWhiteSpace(nyKontakt))
                        valgtSpil.Kontaktperson = nyKontakt;
                }

                SpilDataHandler.GemTilFil(filsti, spilListe);
                Console.Clear();
                Console.WriteLine(newHeader);
                Console.WriteLine("Spillet er blevet opdateret og gemt.");
                ConsoleHelper.VentPåA();
                return;
            }
        }
    }
}