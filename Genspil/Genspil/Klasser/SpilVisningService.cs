using Genspil.Data;

namespace Genspil.Klasser
{
    public static class SpilVisningService
    {
        // Bruges fra hovedmenuen: her må man også reservere
        public static void VisAlleSpil(string filsti, List<Spil> spilListe)
        {
            VisAlleSpilIntern(filsti, spilListe, true);
        }

        // Bruges fx fra søgning: her vises listen kun
        public static void VisAlleSpil(List<Spil> spilListe)
        {
            VisAlleSpilIntern("", spilListe, false);
        }

        private static void VisAlleSpilIntern(string filsti, List<Spil> spilListe, bool måReservere)
        {
            if (spilListe.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("=== Liste over spil ===");
                Console.WriteLine("Ingen spil fundet.");
                ConsoleHelper.VentPåA();
                return;
            }

            char sortering = 'N';

            while (true)
            {
                Console.Clear();
                List<Spil> sorteretListe = HentSorteretListe(spilListe, sortering);

                Console.WriteLine($"=== Liste over spil ({HentSorteringsTekst(sortering)}) ===");
                PrintSpilTabel(sorteretListe);
                Console.WriteLine();

                if (måReservere)
                {
                    Console.WriteLine("Sortér efter: [N]avn | [G]enre | [S]tand | [P]ris | [O]prettelsesdato | [L]spillere");
                    Console.WriteLine("Reservation: [R] Reserver spil | [F] Fjern reservation | [A] Afbryd");
                    Console.Write("\nIndtast spillets ID og tast derefter R eller F: ");

                    (char valg, int id) = LæsKommando();

                    switch (valg)
                    {
                        case 'A':
                            return;
                        case 'N':
                        case 'G':
                        case 'S':
                        case 'P':
                        case 'O':
                        case 'L':
                            sortering = valg;
                            break;
                        case 'R':
                            OpdaterReservation(filsti, spilListe, id, true);
                            break;
                        case 'F':
                            OpdaterReservation(filsti, spilListe, id, false);
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Sortér efter: [N]avn | [G]enre | [S]tand | [P]ris | [O]prettelsesdato | [L]spillere | [A]fbryd");
                    char valg = char.ToUpper(Console.ReadKey(true).KeyChar);

                    if (valg == 'A')
                        return;

                    if ("NGSPOL".Contains(valg))
                        sortering = valg;
                }
            }
        }

        // Sætter eller fjerner reservation og gemmer bagefter
        private static void OpdaterReservation(string filsti, List<Spil> spilListe, int id, bool skalReserveres)
        {
            Spil? valgtSpil = spilListe.Find(s => s.Id == id);

            if (valgtSpil != null)
            {
                if (skalReserveres)
                {
                    Console.Write("Reserveret af (navn): ");
                    string navn = (Console.ReadLine() ?? "").Trim();
                    valgtSpil.SætReservation(navn);
                    Console.WriteLine("Spillet er nu markeret som reserveret.");
                }
                else
                {
                    valgtSpil.FjernReservation();
                    Console.WriteLine("Reservationen er fjernet.");
                }

                SpilDataHandler.GemTilFil(filsti, spilListe);
            }
            else
            {
                Console.WriteLine("Ingen spil fundet med det ID.");
            }

            ConsoleHelper.Pause();
        }

        // Læser fx N, A, 4R eller 4F
        private static (char valg, int id) LæsKommando()
        {
            string idTekst = "";

            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                char tegn = char.ToUpper(keyInfo.KeyChar);

                if (char.IsDigit(keyInfo.KeyChar))
                {
                    idTekst += keyInfo.KeyChar;
                    Console.Write(keyInfo.KeyChar);
                }
                else if (keyInfo.Key == ConsoleKey.Backspace && idTekst.Length > 0)
                {
                    idTekst = idTekst.Substring(0, idTekst.Length - 1);
                    Console.Write("\b \b");
                }
                else if (idTekst == "" && "NGSPOAL".Contains(tegn))
                {
                    Console.WriteLine(tegn);
                    return (tegn, 0);
                }
                else if (idTekst != "" && (tegn == 'R' || tegn == 'F'))
                {
                    Console.WriteLine(tegn);
                    return (tegn, int.Parse(idTekst));
                }
            }
        }

        // Returnerer en sorteret kopi af listen
        public static List<Spil> HentSorteretListe(List<Spil> spilListe, char sortering)
        {
            List<Spil> sorteret = new List<Spil>(spilListe);

            switch (char.ToUpper(sortering))
            {
                case 'G':
                    sorteret.Sort((s1, s2) => s1.Genre.ToString().CompareTo(s2.Genre.ToString()));
                    break;
                case 'S':
                    sorteret.Sort((s1, s2) => s1.Stand.CompareTo(s2.Stand));
                    break;
                case 'P':
                    sorteret.Sort((s1, s2) => s1.Pris.CompareTo(s2.Pris));
                    break;
                case 'O':
                    sorteret.Sort((s1, s2) => s1.Id.CompareTo(s2.Id));
                    break;
                case 'L':
                    sorteret.Sort((s1, s2) =>
                    {
                        int min1 = int.TryParse(s1.AntalSpillere.Split('-')[0], out int a) ? a : 0;
                        int min2 = int.TryParse(s2.AntalSpillere.Split('-')[0], out int b) ? b : 0;
                        return min1.CompareTo(min2);
                    });
                    break;
                default:
                    sorteret.Sort((s1, s2) => s1.Titel.CompareTo(s2.Titel));
                    break;
            }

            return sorteret;
        }

        public static string HentSorteringsTekst(char sortering)
        {
            switch (char.ToUpper(sortering))
            {
                case 'G':
                    return "sorteret efter genre";
                case 'S':
                    return "sorteret efter stand";
                case 'P':
                    return "sorteret efter pris";
                case 'O':
                    return "sorteret efter oprettelsesdato";
                case 'L':
                    return "sorteret efter antal spillere";
                default:
                    return "sorteret efter navn";
            }
        }

        public static void PrintSpilTabel(List<Spil> spilListe)
        {
            string afskæringsstring = new string('-', 120);
            Console.WriteLine(afskæringsstring);
            Console.WriteLine($"{"ID",-5}{"Titel",-33}{"Genre",-15}{"Spillere",-12}{"Stand",-15}{"Pris",8}{"Status",32}");
            Console.WriteLine(afskæringsstring);

            foreach (Spil spil in spilListe)
            {
                Console.WriteLine(spil.VisInfo());
            }

            Console.WriteLine(afskæringsstring);
        }

        public static void VisForespørgsler(List<Spil> spilListe)
        {
            List<Spil> forespørgsler = spilListe.FindAll(s => s.ErRequest);

            if (forespørgsler.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("=== Forespurgte spil ===");
                Console.WriteLine("Ingen forespørgsler registreret.");
                ConsoleHelper.VentPåA();
                return;
            }

            VisAlleSpil(forespørgsler);
        }
    }
}