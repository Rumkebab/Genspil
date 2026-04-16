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
                    Console.WriteLine("Sortér efter: [N]avn | [G]enre | [S]tand | [P]ris | [O]prettelsesdato");
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
                    Console.WriteLine("Sortér efter: [N]avn | [G]enre | [S]tand | [P]ris | [O]prettelsesdato | [A]fbryd");
                    char valg = char.ToUpper(Console.ReadKey(true).KeyChar);

                    if (valg == 'A')
                        return;

                    if ("NGSPO".Contains(valg))
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
                valgtSpil.ErReserveret = skalReserveres;
                SpilDataHandler.GemTilFil(filsti, spilListe);

                if (skalReserveres)
                    Console.WriteLine("Spillet er nu markeret som reserveret.");
                else
                    Console.WriteLine("Reservationen er fjernet.");
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
                else if (idTekst == "" && "NGSPOA".Contains(tegn))
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
                    sorteret.Sort((s1, s2) => s1.Stand.ToString().CompareTo(s2.Stand.ToString()));
                    break;
                case 'P':
                    sorteret.Sort((s1, s2) => s1.Pris.CompareTo(s2.Pris));
                    break;
                case 'O':
                    sorteret.Sort((s1, s2) => s1.Id.CompareTo(s2.Id));
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
                default:
                    return "sorteret efter navn";
            }
        }

        // Udskriver listen i samme tabel-format som Anders’ version
        public static void PrintSpilTabel(List<Spil> spilListe)
        {
            Console.WriteLine(new string('-', 120));
            Console.WriteLine($"{"ID",-5}{"Titel",-50}{"Genre",-15}{"Spillere",-12}{"Stand",-15}{"Pris",8}{"Status",15}");
            Console.WriteLine(new string('-', 120));

            foreach (Spil spil in spilListe)
            {
                Console.WriteLine(spil.VisInfo());
            }

            Console.WriteLine(new string('-', 120));
        }
    }
}