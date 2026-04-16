namespace Genspil.Klasser // Angiver at denne serviceklasse hører til i projektets namespace Genspil
{
    public static class SpilVisningService // Opretter en statisk serviceklasse til visning og sortering af spil
    {
        public static void VisAlleSpil(List<Spil> spilListe) // Metode der viser alle spil og lader brugeren sortere listen
        {
            if (spilListe.Count == 0) // Tjekker om listen er tom
            {
                Console.Clear(); // Rydder konsollen
                Console.WriteLine("=== Liste over spil ==="); // Viser overskrift
                Console.WriteLine("Ingen spil fundet."); // Fortæller brugeren at der ikke findes nogen spil
                ConsoleHelper.VentPåA(); // Venter på at brugeren skriver A for at gå tilbage
                return; // Afslutter metoden
            }

            char sortering = 'N'; // Sætter standard-sorteringen til N, som betyder navn/titel

            while (true) // Løkke der fortsætter indtil brugeren afbryder med A
            {
                Console.Clear(); // Rydder konsollen før listen vises igen
                List<Spil> sorteretListe = HentSorteretListe(spilListe, sortering); // Henter en sorteret kopi af listen ud fra den valgte sortering

                Console.WriteLine($"=== Liste over spil ({HentSorteringsTekst(sortering)}) ==="); // Viser overskrift med tekst for den aktuelle sortering
                PrintSpilTabel(sorteretListe); // Udskriver listen pænt som tabel

                Console.WriteLine(); // Skriver en tom linje for luft i layoutet
                Console.WriteLine("Sortér efter: [N]avn | [G]enre | [S]tand | [P]ris | [O]prettelsesdato | [A]fbryd"); // Viser sorteringsmuligheder
                Console.Write("> "); // Inputmarkør

                char valg = char.ToUpper(Console.ReadKey(true).KeyChar); // Læser ét tegn fra brugeren og gør det til stort bogstav

                if (valg == 'A') // Hvis brugeren vælger A
                {
                    return; // Afslutter metoden og går tilbage
                }

                if ("NGSPO".Contains(valg)) // Tjekker om bogstavet er en gyldig sorteringsmulighed
                {
                    sortering = valg; // Gemmer den nye sortering, så listen vises anderledes næste gang
                }
            }
        }

        public static List<Spil> HentSorteretListe(List<Spil> spilListe, char sortering) // Metode der returnerer en ny liste sorteret efter brugerens valg
        {
            List<Spil> sorteret = new List<Spil>(spilListe); // Opretter en kopi af listen, så originalen ikke bliver ændret direkte

            switch (char.ToUpper(sortering)) // Kigger på hvilken sortering der er valgt
            {
                case 'G': // Hvis der sorteres efter genre
                    sorteret.Sort((s1, s2) => s1.Genre.ToString().CompareTo(s2.Genre.ToString())); // Sorterer alfabetisk på genre
                    break; // Afslutter denne case

                case 'S': // Hvis der sorteres efter stand
                    sorteret.Sort((s1, s2) => s1.Stand.ToString().CompareTo(s2.Stand.ToString())); // Sorterer alfabetisk på stand
                    break; // Afslutter denne case

                case 'P': // Hvis der sorteres efter pris
                    sorteret.Sort((s1, s2) => s1.Pris.CompareTo(s2.Pris)); // Sorterer numerisk efter pris
                    break; // Afslutter denne case

                case 'O': // Hvis der sorteres efter oprettelsesdato, som i jeres projekt svarer til ID-rækkefølge
                    sorteret.Sort((s1, s2) => s1.Id.CompareTo(s2.Id)); // Sorterer stigende efter ID
                    break; // Afslutter denne case

                case 'N': // Hvis der sorteres efter navn
                default: // Standardvalg hvis input ikke passer på andre cases
                    sorteret.Sort((s1, s2) => s1.Titel.CompareTo(s2.Titel)); // Sorterer alfabetisk efter titel
                    break; // Afslutter denne case
            }

            return sorteret; // Returnerer den sorterede kopi af listen
        }

        public static string HentSorteringsTekst(char sortering) // Metode der laver en pæn tekstbeskrivelse af den valgte sortering
        {
            switch (char.ToUpper(sortering)) // Kigger på sorteringsbogstavet
            {
                case 'G': // Hvis sortering er genre
                    return "sorteret efter genre"; // Returnerer tekst til visning i overskriften
                case 'S': // Hvis sortering er stand
                    return "sorteret efter stand"; // Returnerer tekst til visning i overskriften
                case 'P': // Hvis sortering er pris
                    return "sorteret efter pris"; // Returnerer tekst til visning i overskriften
                case 'O': // Hvis sortering er oprettelsesdato/ID
                    return "sorteret efter oprettelsesdato"; // Returnerer tekst til visning i overskriften
                case 'N': // Hvis sortering er navn
                default: // Standardvalg
                    return "sorteret efter navn"; // Returnerer tekst til visning i overskriften
            }
        }

        public static void PrintSpilTabel(List<Spil> spilListe) // Metode der udskriver spillene i en pæn tabel
        {
            Console.WriteLine(new string('-', 118)); // Skriver en vandret streg som topkant på tabellen
            Console.WriteLine($"{"ID",-5} {"Titel",-38} {"Genre",-15} {"Stand",-10} {"Pris",-10} {"Status",-25}"); // Skriver kolonneoverskrifter med fast bredde
            Console.WriteLine(new string('-', 118)); // Skriver endnu en vandret streg under overskrifterne

            foreach (Spil spil in spilListe) // Går igennem hvert spil i listen
            {
                string prisTekst = $"{spil.Pris} kr"; // Laver prisen om til en tekst med "kr"
                string status = HentStatusTekst(spil); // Henter status-tekst for spillet, fx reserveret eller ønsket

                Console.WriteLine( // Skriver én række i tabellen
                    $"{spil.Id,-5} " + // Viser ID i en kolonne på 5 tegn
                    $"{AfkortTekst(spil.Titel, 38),-38} " + // Viser titel og afkorter hvis den er for lang
                    $"{spil.Genre,-15} " + // Viser genre i kolonne på 15 tegn
                    $"{spil.Stand,-10} " + // Viser stand i kolonne på 10 tegn
                    $"{prisTekst,-10} " + // Viser pris i kolonne på 10 tegn
                    $"{status,-25}" // Viser status i kolonne på 25 tegn
                );
            }

            Console.WriteLine(new string('-', 118)); // Skriver en bundlinje på tabellen
        }

        public static string HentStatusTekst(Spil spil) // Metode der finder tekst til statuskolonnen
        {
            List<string> statusListe = new List<string>(); // Opretter en liste til at samle status-ord

            if (spil.ErReserveret) // Tjekker om spillet er reserveret
            {
                statusListe.Add("(RESERVERET)"); // Tilføjer reserveret-status til listen
            }

            if (spil.ErRequest) // Tjekker om spillet er markeret som ønsket/request
            {
                statusListe.Add("(ØNSKET)"); // Tilføjer ønsket-status til listen
            }

            return string.Join(" ", statusListe); // Samler alle status-ord til én tekst med mellemrum imellem
        }

        public static string AfkortTekst(string tekst, int maxLængde) // Metode der forkorter en tekst hvis den er længere end den tilladte længde
        {
            if (string.IsNullOrWhiteSpace(tekst)) // Tjekker om teksten er tom eller kun mellemrum
            {
                return ""; // Returnerer tom tekst hvis der ikke er noget indhold
            }

            if (tekst.Length <= maxLængde) // Tjekker om teksten allerede er kort nok
            {
                return tekst; // Returnerer teksten uændret
            }

            return tekst.Substring(0, maxLængde - 3) + "..."; // Skærer teksten af og tilføjer ... til sidst
        }
    }
}