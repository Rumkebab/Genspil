using Genspil.Klasser; // Giver adgang til Spil, Genre og Stand
using Genspil.Data;    // Giver adgang til SpilDataHandler

// Sti til filen med spillene
string filsti = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Datafiler\spil.txt");
filsti = Path.GetFullPath(filsti);

// Læser spillene fra filen ved opstart
List<Spil> spilListe = SpilDataHandler.LæsFraFil(filsti);

// Starter hovedmenuen
Hovedmenu(filsti, spilListe);

static void Hovedmenu(string filsti, List<Spil> spilListe)
{
    bool kører = true;

    while (kører)
    {
        Console.Clear();
        Console.WriteLine("==================================");
        Console.WriteLine("          GENSPIL MENU            ");
        Console.WriteLine("==================================");
        Console.WriteLine("1. Vis alle spil");
        Console.WriteLine("2. Tilføj nyt spil");
        Console.WriteLine("3. Slet spil");
        Console.WriteLine("4. Rediger spil");
        Console.WriteLine("5. Søg efter spil");
        Console.WriteLine("6. Afslut");
        Console.WriteLine("==================================");
        Console.Write("Vælg en mulighed: ");

        string valg = (Console.ReadLine() ?? "").Trim();

        switch (valg)
        {
            case "1":
                VisAlleSpil(spilListe);
                break;

            case "2":
                bool fortsætTilføjelse = true;

                while (fortsætTilføjelse)
                {
                    Spil? nytSpil = OpretNytSpil();

                    if (nytSpil == null)
                    {
                        fortsætTilføjelse = false;
                        break;
                    }

                    spilListe.Add(nytSpil);

                    // Gemmer automatisk efter oprettelse
                    SpilDataHandler.GemTilFil(filsti, spilListe);

                    Console.WriteLine();
                    Console.WriteLine("Spillet blev tilføjet og gemt.");
                    Console.WriteLine("==================================");
                    Console.WriteLine("1. Tilføj endnu et spil");
                    Console.WriteLine("A. Afbryd og vend tilbage til hovedmenuen");
                    Console.WriteLine("==================================");

                    string tilfoejValg = "";
                    while (tilfoejValg != "1" && tilfoejValg != "A")
                    {
                        Console.Write("Valg: ");
                        tilfoejValg = (Console.ReadLine() ?? "").Trim().ToUpper();

                        if (tilfoejValg != "1" && tilfoejValg != "A")
                        {
                            Console.WriteLine("Ugyldigt valg. Indtast 1 eller A.");
                        }
                    }

                    if (tilfoejValg == "A")
                    {
                        fortsætTilføjelse = false;
                    }
                }
                break;

            case "3":
                SletSpil(filsti, spilListe);
                break;

            case "4":
                RedigerSpil(filsti, spilListe);
                break;

            case "5":
                SøgEfterSpilMenu(spilListe);
                break;

            case "6":
                kører = false;
                break;

            default:
                Console.WriteLine();
                Console.WriteLine("Ugyldigt valg. Prøv igen.");
                Pause();
                break;
        }
    }
}

// =========================
// VISNING OG SORTERING
// =========================

static void VisAlleSpil(List<Spil> spilListe)
{
    if (spilListe.Count == 0)
    {
        Console.Clear();
        Console.WriteLine("=== Liste over spil ===");
        Console.WriteLine("Ingen spil fundet.");
        VentPåA();
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
        Console.WriteLine("Sortér efter: [N]avn | [G]enre | [S]tand | [P]ris | [O]prettelsesdato | [A]fbryd");
        Console.Write("> ");

        char valg = char.ToUpper(Console.ReadKey(true).KeyChar);

        if (valg == 'A')
        {
            return;
        }

        if ("NGSPO".Contains(valg))
        {
            sortering = valg;
        }
    }
}

static List<Spil> HentSorteretListe(List<Spil> spilListe, char sortering)
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

static string HentSorteringsTekst(char sortering)
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

static void PrintSpilTabel(List<Spil> spilListe)
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

static string HentStatusTekst(Spil spil)
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

static string AfkortTekst(string tekst, int maxLængde)
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

// =========================
// OPRET
// =========================

static Spil? OpretNytSpil()
{
    Console.Clear();
    Console.WriteLine("=== Opret nyt spil === Tast A for at afbryde");

    Console.Write("Indtast titel: ");
    string titel = (Console.ReadLine() ?? "").Trim();

    if (titel.ToUpper() == "A")
    {
        return null;
    }

    Console.WriteLine("Vælg genre:");
    Console.WriteLine("1 = Strategi");
    Console.WriteLine("2 = Familie");
    Console.WriteLine("3 = Kortspil");
    Console.WriteLine("4 = Quiz");
    Console.WriteLine("5 = Samarbejde");
    Console.Write("Vælg genre: ");

    string genreInput = (Console.ReadLine() ?? "").Trim();
    if (genreInput.ToUpper() == "A")
    {
        return null;
    }

    if (!int.TryParse(genreInput, out int genreValg) || genreValg < 1 || genreValg > 5)
    {
        Console.WriteLine("Ugyldigt genrevalg.");
        Pause();
        return null;
    }

    Genre genre = (Genre)(genreValg - 1);

    Console.WriteLine("Vælg stand:");
    Console.WriteLine("1 = Ny");
    Console.WriteLine("2 = God");
    Console.WriteLine("3 = Slidt");
    Console.Write("Vælg stand: ");

    string standInput = (Console.ReadLine() ?? "").Trim();
    if (standInput.ToUpper() == "A")
    {
        return null;
    }

    if (!int.TryParse(standInput, out int standValg) || standValg < 1 || standValg > 3)
    {
        Console.WriteLine("Ugyldigt standvalg.");
        Pause();
        return null;
    }

    Stand stand = (Stand)(standValg - 1);

    Console.Write("Indtast pris: ");
    string prisInput = (Console.ReadLine() ?? "").Trim();

    if (prisInput.ToUpper() == "A")
    {
        return null;
    }

    if (!int.TryParse(prisInput, out int pris))
    {
        Console.WriteLine("Ugyldig pris.");
        Pause();
        return null;
    }

    return new Spil(titel, genre, stand, pris);
}

// =========================
// SLET
// =========================

static void SletSpil(string filsti, List<Spil> spilListe)
{
    if (spilListe.Count == 0)
    {
        Console.Clear();
        Console.WriteLine("=== Slet spil === Tast A for at afbryde");
        Console.WriteLine("Ingen spil at slette.");
        VentPåA();
        return;
    }

    char sortering = 'O';

    while (true)
    {
        Console.Clear();
        Console.WriteLine("=== Slet spil === Tast A for at afbryde");

        List<Spil> sorteretListe = HentSorteretListe(spilListe, sortering);
        PrintSpilTabel(sorteretListe);

        Console.WriteLine("Sortér efter: [N]avn | [G]enre | [S]tand | [P]ris | [O]prettelsesdato | [A]fbryd");
        Console.Write("Indtast ID på spil der skal slettes: ");

        string input = (Console.ReadLine() ?? "").Trim();

        if (input.ToUpper() == "A")
        {
            return;
        }

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

                Console.WriteLine();
                Console.WriteLine("Spillet er slettet og gemt.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Ingen spil fundet med det ID.");
            }

            VentPåA();
            return;
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine("Du skal skrive et gyldigt ID eller et sorteringsbogstav.");
            Pause();
        }
    }
}

// =========================
// REDIGER
// =========================

static void RedigerSpil(string filsti, List<Spil> spilListe)
{
    if (spilListe.Count == 0)
    {
        Console.Clear();
        Console.WriteLine("=== Rediger spil === Tast A for at afbryde");
        Console.WriteLine("Ingen spil at redigere.");
        VentPåA();
        return;
    }

    char sortering = 'O';

    while (true)
    {
        Console.Clear();
        Console.WriteLine("=== Rediger spil === Tast A for at afbryde");

        List<Spil> sorteretListe = HentSorteretListe(spilListe, sortering);
        PrintSpilTabel(sorteretListe);

        Console.WriteLine("Sortér efter: [N]avn | [G]enre | [S]tand | [P]ris | [O]prettelsesdato | [A]fbryd");
        Console.Write("Indtast ID på spil der skal redigeres: ");

        string input = (Console.ReadLine() ?? "").Trim();

        if (input.ToUpper() == "A")
        {
            return;
        }

        if (input.Length == 1 && "NGSPO".Contains(char.ToUpper(input[0])))
        {
            sortering = char.ToUpper(input[0]);
            continue;
        }

        if (int.TryParse(input, out int id))
        {
            Spil? valgtSpil = spilListe.Find(s => s.Id == id);

            if (valgtSpil == null)
            {
                Console.WriteLine();
                Console.WriteLine("Ingen spil fundet med det ID.");
                VentPåA();
                return;
            }

            Console.WriteLine();
            Console.WriteLine("Tryk Enter hvis du vil beholde den nuværende værdi.");
            Console.WriteLine("Tast A for at afbryde.");
            Console.WriteLine("==================================");

            Console.Write($"Ny titel ({valgtSpil.Titel}): ");
            string nyTitel = (Console.ReadLine() ?? "").Trim();

            if (nyTitel.ToUpper() == "A")
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(nyTitel))
            {
                valgtSpil.Titel = nyTitel;
            }

            Console.WriteLine();
            Console.WriteLine($"Nuværende genre: {valgtSpil.Genre}");
            Console.WriteLine("1 = Strategi");
            Console.WriteLine("2 = Familie");
            Console.WriteLine("3 = Kortspil");
            Console.WriteLine("4 = Quiz");
            Console.WriteLine("5 = Samarbejde");
            Console.Write("Vælg ny genre (tryk Enter for at beholde): ");
            string genreInput = (Console.ReadLine() ?? "").Trim();

            if (genreInput.ToUpper() == "A")
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(genreInput))
            {
                if (int.TryParse(genreInput, out int genreValg))
                {
                    if (genreValg >= 1 && genreValg <= 5)
                    {
                        valgtSpil.Genre = (Genre)(genreValg - 1);
                    }
                    else
                    {
                        Console.WriteLine("Ugyldigt genrevalg. Den gamle genre beholdes.");
                    }
                }
                else
                {
                    Console.WriteLine("Ugyldigt input. Den gamle genre beholdes.");
                }
            }

            Console.WriteLine();
            Console.WriteLine($"Nuværende stand: {valgtSpil.Stand}");
            Console.WriteLine("1 = Ny");
            Console.WriteLine("2 = God");
            Console.WriteLine("3 = Slidt");
            Console.Write("Vælg ny stand (tryk Enter for at beholde): ");
            string standInput = (Console.ReadLine() ?? "").Trim();

            if (standInput.ToUpper() == "A")
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(standInput))
            {
                if (int.TryParse(standInput, out int standValg))
                {
                    if (standValg >= 1 && standValg <= 3)
                    {
                        valgtSpil.Stand = (Stand)(standValg - 1);
                    }
                    else
                    {
                        Console.WriteLine("Ugyldigt standvalg. Den gamle stand beholdes.");
                    }
                }
                else
                {
                    Console.WriteLine("Ugyldigt input. Den gamle stand beholdes.");
                }
            }

            Console.WriteLine();
            Console.Write($"Ny pris ({valgtSpil.Pris} kr): ");
            string prisInput = (Console.ReadLine() ?? "").Trim();

            if (prisInput.ToUpper() == "A")
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(prisInput))
            {
                if (int.TryParse(prisInput, out int nyPris))
                {
                    valgtSpil.Pris = nyPris;
                }
                else
                {
                    Console.WriteLine("Ugyldig pris. Den gamle pris beholdes.");
                }
            }

            SpilDataHandler.GemTilFil(filsti, spilListe);

            Console.WriteLine();
            Console.WriteLine("Spillet er blevet opdateret og gemt.");
            VentPåA();
            return;
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine("Du skal skrive et gyldigt ID eller et sorteringsbogstav.");
            Pause();
        }
    }
}

// =========================
// SØGNING
// =========================

static void SøgEfterSpilMenu(List<Spil> spilListe)
{
    Console.Clear();
    Console.WriteLine("=== Vælg hvad du vil søge efter ===");
    Console.WriteLine("1. Navn");
    Console.WriteLine("2. Genre");
    Console.WriteLine("3. Pris");
    Console.WriteLine("4. Stand");
    Console.WriteLine("5. Søg på flere kriterier");
    Console.WriteLine("A. Gå tilbage");
    Console.Write("> ");

    string svalg = (Console.ReadLine() ?? "").Trim().ToUpper();

    switch (svalg)
    {
        case "1":
            SøgEfterSpil(spilListe, "Titel");
            break;
        case "2":
            SøgEfterSpil(spilListe, "Genre");
            break;
        case "3":
            SøgEfterSpil(spilListe, "Pris");
            break;
        case "4":
            SøgEfterSpil(spilListe, "Stand");
            break;
        case "5":
            SøgEfterFlereKriterier(spilListe);
            break;
        case "A":
            return;
        default:
            Console.WriteLine("Ugyldigt input. Prøv igen.");
            Pause();
            break;
    }
}

static void SøgEfterSpil(List<Spil> spilListe, string svalue = "Titel")
{
    List<Spil> fundneSpil = new List<Spil>();

    if (spilListe.Count == 0)
    {
        Console.WriteLine("Ingen spil at søge efter.");
        VentPåA();
        return;
    }

    Console.Clear();

    switch (svalue)
    {
        case "Titel":
            Console.WriteLine("=== Søg efter navn === Tast A for at afbryde");
            Console.Write("Indtast titel eller en del af titlen: ");
            break;
        case "Genre":
            Console.WriteLine("=== Søg efter genre === Tast A for at afbryde");
            Console.Write("Indtast genre: ");
            break;
        case "Pris":
            Console.WriteLine("=== Søg efter pris === Tast A for at afbryde");
            Console.Write("Indtast pris: ");
            break;
        case "Stand":
            Console.WriteLine("=== Søg efter stand === Tast A for at afbryde");
            Console.Write("Indtast stand: ");
            break;
        default:
            Console.WriteLine("=== Søg efter navn === Tast A for at afbryde");
            Console.Write("Indtast titel eller en del af titlen: ");
            break;
    }

    string søgning = (Console.ReadLine() ?? "").Trim();

    if (søgning.ToUpper() == "A")
    {
        return;
    }

    if (string.IsNullOrWhiteSpace(søgning))
    {
        Console.WriteLine("Du skal skrive noget for at søge.");
        VentPåA();
        return;
    }

    foreach (Spil spil in spilListe)
    {
        if (svalue == "Titel" && spil.Titel.ToLower().Contains(søgning.ToLower()))
        {
            fundneSpil.Add(spil);
        }
        else if (svalue == "Genre" && spil.Genre.ToString().ToLower().Contains(søgning.ToLower()))
        {
            fundneSpil.Add(spil);
        }
        else if (svalue == "Pris" && spil.Pris.ToString().Contains(søgning))
        {
            fundneSpil.Add(spil);
        }
        else if (svalue == "Stand" && spil.Stand.ToString().ToLower().Contains(søgning.ToLower()))
        {
            fundneSpil.Add(spil);
        }
    }

    Console.WriteLine();

    if (fundneSpil.Count == 0)
    {
        Console.WriteLine("Ingen spil fundet.");
    }
    else
    {
        Console.WriteLine("Fundne spil:");
        PrintSpilTabel(fundneSpil);
    }

    VentPåA();
}

static void SøgEfterFlereKriterier(List<Spil> spilListe)
{
    Console.Clear();
    Console.WriteLine("=== Søg på flere kriterier === Tast A for at afbryde");

    Console.Write("Titel (tryk Enter for at springe over): ");
    string titel = (Console.ReadLine() ?? "").Trim();
    if (titel.ToUpper() == "A") return;

    Console.Write("Genre (tryk Enter for at springe over): ");
    string genre = (Console.ReadLine() ?? "").Trim();
    if (genre.ToUpper() == "A") return;

    Console.Write("Stand (tryk Enter for at springe over): ");
    string stand = (Console.ReadLine() ?? "").Trim();
    if (stand.ToUpper() == "A") return;

    Console.Write("Pris (tryk Enter for at springe over): ");
    string pris = (Console.ReadLine() ?? "").Trim();
    if (pris.ToUpper() == "A") return;

    List<Spil> fundneSpil = new List<Spil>();

    foreach (Spil spil in spilListe)
    {
        bool matcherTitel = string.IsNullOrWhiteSpace(titel) || spil.Titel.ToLower().Contains(titel.ToLower());
        bool matcherGenre = string.IsNullOrWhiteSpace(genre) || spil.Genre.ToString().ToLower().Contains(genre.ToLower());
        bool matcherStand = string.IsNullOrWhiteSpace(stand) || spil.Stand.ToString().ToLower().Contains(stand.ToLower());
        bool matcherPris = string.IsNullOrWhiteSpace(pris) || spil.Pris.ToString().Contains(pris);

        if (matcherTitel && matcherGenre && matcherStand && matcherPris)
        {
            fundneSpil.Add(spil);
        }
    }

    Console.WriteLine();

    if (fundneSpil.Count == 0)
    {
        Console.WriteLine("Ingen spil fundet.");
    }
    else
    {
        Console.WriteLine("Fundne spil:");
        PrintSpilTabel(fundneSpil);
    }

    VentPåA();
}

// =========================
// HJÆLPEMETODER
// =========================

static void Pause()
{
    Console.WriteLine();
    Console.WriteLine("Tryk på en tast for at fortsætte...");
    Console.ReadKey();
}

static void VentPåA(string promptTekst = "Indtast A for at vende tilbage: ")
{
    while (true)
    {
        Console.Write(promptTekst);
        string input = (Console.ReadLine() ?? "").Trim().ToUpper();

        if (input == "A")
        {
            return;
        }
    }
}