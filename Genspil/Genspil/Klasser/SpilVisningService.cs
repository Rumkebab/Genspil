using Genspil.Klasser;
using Genspil.Data;
using System.Text;

namespace Genspil
{
    public static class SpilVisningService
    {
        public static void VisAlleSpil(string filsti, List<Spil> spilListe)
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
                Console.WriteLine("Sortér efter: [N]avn | [G]enre | [S]tand | [P]ris | [O]prettelsesdato");
                Console.WriteLine("Reservation: [R] Reserver spil | [F] Fjern reservation | [A] Afbryd");
                Console.Write("Indtast spillets ID og tast derefter R eller F: ");

                var kommando = LæsVisningsKommando();

                if (kommando.Type == "Afbryd")
                {
                    return;
                }

                if (kommando.Type == "Sorter")
                {
                    sortering = kommando.Bogstav;
                    continue;
                }

                if (kommando.Type == "Reserver")
                {
                    OpdaterReservation(filsti, spilListe, kommando.Id, true);
                    continue;
                }

                if (kommando.Type == "Fjern")
                {
                    OpdaterReservation(filsti, spilListe, kommando.Id, false);
                    continue;
                }
            }
        }

        private static void OpdaterReservation(string filsti, List<Spil> spilListe, int id, bool skalReserveres)
        {
            Spil? valgtSpil = spilListe.Find(s => s.Id == id);

            if (valgtSpil != null)
            {
                valgtSpil.ErReserveret = skalReserveres;
                SpilDataHandler.GemTilFil(filsti, spilListe);

                if (skalReserveres)
                {
                    Console.WriteLine("Spillet er nu markeret som reserveret.");
                }
                else
                {
                    Console.WriteLine("Reservationen er fjernet.");
                }
            }
            else
            {
                Console.WriteLine("Ingen spil fundet med det ID.");
            }

            ConsoleHelper.Pause();
        }

        private static (string Type, char Bogstav, int Id) LæsVisningsKommando()
        {
            StringBuilder idBuffer = new StringBuilder();

            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                char tegn = char.ToUpper(keyInfo.KeyChar);

                if (char.IsDigit(keyInfo.KeyChar))
                {
                    idBuffer.Append(keyInfo.KeyChar);
                    Console.Write(keyInfo.KeyChar);
                    continue;
                }

                if (keyInfo.Key == ConsoleKey.Backspace && idBuffer.Length > 0)
                {
                    idBuffer.Length--;
                    Console.Write("\b \b");
                    continue;
                }

                if (idBuffer.Length == 0 && "NGSPO".Contains(tegn))
                {
                    Console.WriteLine(tegn);
                    return ("Sorter", tegn, 0);
                }

                if (tegn == 'A')
                {
                    Console.WriteLine("A");
                    return ("Afbryd", 'A', 0);
                }

                if (idBuffer.Length > 0 && tegn == 'R')
                {
                    Console.WriteLine("R");
                    return ("Reserver", 'R', int.Parse(idBuffer.ToString()));
                }

                if (idBuffer.Length > 0 && tegn == 'F')
                {
                    Console.WriteLine("F");
                    return ("Fjern", 'F', int.Parse(idBuffer.ToString()));
                }
            }
        }

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

                case 'N':
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
                case 'N':
                default:
                    return "sorteret efter navn";
            }
        }

        public static void PrintSpilTabel(List<Spil> spilListe)
        {
            Console.WriteLine(new string('-', 118));
            Console.WriteLine($"{"ID",-5} {"Titel",-38} {"Genre",-15} {"Stand",-10} {"Pris",-10} {"Status",-25}");
            Console.WriteLine(new string('-', 118));

            foreach (Spil spil in spilListe)
            {
                string prisTekst = $"{spil.Pris} kr";
                string status = HentStatusTekst(spil);

                Console.WriteLine(
                    $"{spil.Id,-5} " +
                    $"{AfkortTekst(spil.Titel, 38),-38} " +
                    $"{spil.Genre,-15} " +
                    $"{spil.Stand,-10} " +
                    $"{prisTekst,-10} " +
                    $"{status,-25}"
                );
            }

            Console.WriteLine(new string('-', 118));
        }

        public static string HentStatusTekst(Spil spil)
        {
            List<string> statusListe = new List<string>();

            if (spil.ErReserveret)
            {
                statusListe.Add("(RESERVERET)");
            }

            if (spil.ErRequest)
            {
                statusListe.Add("(ØNSKET)");
            }

            return string.Join(" ", statusListe);
        }

        public static string AfkortTekst(string tekst, int maxLængde)
        {
            if (string.IsNullOrWhiteSpace(tekst))
            {
                return "";
            }

            if (tekst.Length <= maxLængde)
            {
                return tekst;
            }

            return tekst.Substring(0, maxLængde - 3) + "...";
        }
    }
}