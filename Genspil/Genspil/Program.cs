using Genspil.Klasser;
using Genspil.Data;

// Sti til tekstfilen
string filsti = "Datafiler/spil.txt";

// Læser spillene fra filen når programmet starter
List<Spil> spilListe = SpilDataHandler.LæsFraFil(filsti);


static void Hovedmenu(string filsti, List<Spil> spilListe)
{

    // Styrer om programmet skal fortsætte
    bool kører = true;


    // Menu-loop
    while (kører)
    {
        Console.Clear();
        Console.WriteLine("=== Genspil Menu ===");
        Console.WriteLine("1. Vis alle spil");
        Console.WriteLine("2. Tilføj nyt spil");
        Console.WriteLine("3. Slet spil");
        Console.WriteLine("4. Rediger spil");
        Console.WriteLine("5. Søg efter spil");
        Console.WriteLine("6. Afslut");
        Console.Write("Vælg en mulighed: ");

        string valg = Console.ReadKey().KeyChar.ToString();

        switch (valg)
        {
            case "1":
                VisAlleSpil(spilListe, filsti);
                break;

            case "2":
                Spil nytSpil = OpretNytSpil(filsti, spilListe);

                Console.WriteLine("Spillet blev tilføjet.");
                Pause();
                break;
            case "3":
                SletSpil(filsti, spilListe); // Går til en undermenu for at vælge hvilket spil der skal slettes
                break;

            case "4":
                RedigerSpil(filsti, spilListe); // Går til en undermenu for at vælge hvilket
                break;

            case "5":
                SøgEfterSpilMenu(filsti, spilListe); // Går til en undermenu for at vælge søgekriterier
                break;

            case "6":
                kører = false;
                Exit();
                break;

            default:
                Console.WriteLine("Ugyldigt valg. Prøv igen.");
                Pause();
                break;
        }
    }
}
Hovedmenu(filsti, spilListe);

static void GemTilFil(string filsti, List<Spil> spilListe)
{
    SpilDataHandler.GemTilFil(filsti, spilListe); // Gemmer spillene til filen
}
static void PrintSpilListe(List<Spil> spilListe)
{
    // Udskriv tabel-header

    Console.WriteLine(new string('-', 120));
    Console.WriteLine($"{"ID",-5} {"Titel",-50} {"Genre",-12} {"Stand",-8} {"Pris",8}");
    Console.WriteLine(new string('-', 120));

    spilListe.Sort((s1, s2) => s1.Id.CompareTo(s2.Id)); // Sorter efter ID  i stigende rækkefølge for at starte visningen i den rækkefølge spillene blev oprettet (ID er auto-increment)

    // Udskriv hver spil som en tabel-række
    for (int i = 0; i < spilListe.Count; i++)
    {

        Spil spil = spilListe[i];
        Console.WriteLine(spilListe[i].VisInfo());
    }

    Console.WriteLine(new string('-', 120));
}

// Viser alle spil i listen
static void VisAlleSpil(List<Spil> spilListe, string filsti)
{

    Console.Clear();
    Console.WriteLine("=== Liste over spil ===");

    if (spilListe.Count == 0)
    {
        Console.WriteLine("Ingen spil fundet.");
    }
    else
    {
        PrintSpilListe(spilListe);
    }

    void Sorteringsmenu()
    {
        Console.WriteLine($"\nSortér efter: [N]avn | [G]enre | [S]tand | [P]ris | [O]prettelsesdato | | [A]fbryd");
        Console.Write("> ");
        char sortValg = char.ToUpper(Console.ReadKey().KeyChar);
        SorterSpil(sortValg);
    }

    void SorterSpil(char sortValg)
    {
        switch (sortValg)
        {
            case 'G':
                spilListe.Sort((s1, s2) => s1.Genre.CompareTo(s2.Genre)); // Sorter efter genre i stigende rækkefølge
                Console.Clear();
                Console.WriteLine("=== Liste over spil (sorteret efter genre) ===");
                break;
            case 'S':
                spilListe.Sort((s1, s2) => s1.Stand.CompareTo(s2.Stand)); // Sorter efter stand i stigende rækkefølge
                Console.Clear();
                Console.WriteLine("=== Liste over spil (sorteret efter stand) ===");
                break;
            case 'P':
                spilListe.Sort((s1, s2) => s1.Pris.CompareTo(s2.Pris)); // Sorter efter pris i stigende rækkefølge
                Console.Clear();
                Console.WriteLine("=== Liste over spil (sorteret efter pris) ===");
                break;
            case 'O':
                spilListe.Sort((s1, s2) => s1.Id.CompareTo(s2.Id)); // Sorter efter ID i stigende rækkefølge
                Console.Clear();
                Console.WriteLine("=== Liste over spil (sorteret efter pris) ===");
                break;
            case 'A':
                Hovedmenu(filsti, spilListe);
                break;
            default: // Behandl som Navn hvis input er ugyldigt
                spilListe.Sort((s1, s2) => s1.Titel.CompareTo(s2.Titel)); // Sorter alfabetisk efter titel
                Console.Clear();
                Console.WriteLine("=== Liste over spil (sorteret efter navn) ===");
                break;
        }
        // Udskriv tabel-header
        Console.WriteLine(new string('-', 120));
        Console.WriteLine($"{"ID",-5} {"Titel",-50} {"Genre",-12} {"Stand",-8} {"Pris",8}");
        Console.WriteLine(new string('-', 120));

        for (int i = 0; i < spilListe.Count; i++)
        {
            Console.WriteLine(spilListe[i].VisInfo());
        }
        Console.WriteLine(new string('-', 120));

        Sorteringsmenu();
        Pause();
    }

    Sorteringsmenu();
}

// Opretter et nyt spil ud fra brugerens input
static Spil OpretNytSpil(string filsti, List<Spil> spilListe)
{
    Console.Clear();
    Console.WriteLine("=== Opret nyt spil === Tast A for at afbryde");

    Console.Write("Indtast titel: ");
    string titel = Console.ReadLine();
    if (titel.ToUpper() == "A")
    {
        GoBack();
        return null;
    }

    Console.WriteLine("Vælg genre:");
    Console.WriteLine("1 = Strategi");
    Console.WriteLine("2 = Familie");
    Console.WriteLine("3 = Kortspil");
    Console.WriteLine("4 = Quiz");
    Console.WriteLine("5 = Samarbejde");
    var genreValgInput = Console.ReadLine();
    if(genreValgInput.ToUpper() == "A")
    {
        GoBack();
        return null;
    }
    int genreValg = int.Parse(genreValgInput);
    Genre genre = (Genre)(genreValg - 1);

    Console.WriteLine("Vælg stand:");
    Console.WriteLine("1 = Ny");
    Console.WriteLine("2 = God");
    Console.WriteLine("3 = Slidt");
    var standValgInput = Console.ReadLine();
    if (standValgInput.ToUpper() == "A")
    {
        GoBack();
        return null;
    }
    int standValg = int.Parse(standValgInput);
    Stand stand = (Stand)(standValg - 1);

    Console.Write("Indtast pris: ");
    var PrisInput = Console.ReadLine();
    if (PrisInput.ToUpper() == "A")
    {
        GoBack();
        return null;
    }
    int pris = int.Parse(PrisInput);

    Console.Write("Er det et request? (ja/nej): ");
    var requestInput = Console.ReadLine();
    if (requestInput.ToUpper() == "A")
    {
        GoBack();
        return null;
    }
    if (!bool.TryParse(requestInput.Trim().ToLower() == "ja" ? "true" : "false", out bool erRequest))
    {
        erRequest = false; // Standard til false hvis input er ugyldigt
    }
    bool erReserveret = false; // Standard til false for nye spil
    int id = 0; // Id genereres automatisk i Spil-klassen
    Spil nytSpil = new Spil(titel, genre, stand, pris);
    spilListe.Add(nytSpil);
    GemTilFil(filsti, spilListe); // Gem det nye spil til filen med det samme for at sikre at det får tildelt et ID og bliver tilgængeligt i resten af programmet
    return nytSpil;


}

// Sletter et spil fra listen
static void SletSpil(string filsti, List<Spil> spilListe)
{
    Console.Clear();
    Console.WriteLine("=== Slet spil === Tast A for at afbryde");

    if (spilListe.Count == 0)
    {
        Console.WriteLine("Ingen spil at slette.");
        Pause();
        return;
    }

    PrintSpilListe(spilListe);

    Console.Write("Indtast ID på spil der skal slettes: ");
    var sletInput = Console.ReadLine();
    if (sletInput.ToUpper() == "A")
    {
        GoBack();
        return;
    }

    if (int.TryParse(sletInput, out int id))
    {
        Spil spilAtSlette = spilListe.Find(s => s.Id == id);

        if (spilAtSlette != null)
        {
            spilListe.Remove(spilAtSlette);
            Console.WriteLine("Spillet er slettet.");
            GemTilFil(filsti, spilListe);
        }
        else
        {
            Console.WriteLine("Ingen spil fundet med det ID.");
        }
    }
    else
    {
        Console.WriteLine("Du skal skrive et tal.");
    }

    Pause();
}

// Redigerer et spil i listen
static void RedigerSpil(string filsti, List<Spil> spilListe)
{
    Console.Clear();
    Console.WriteLine("=== Rediger spil === Tast A for at afbryde");

    if (spilListe.Count == 0)
    {
        Console.WriteLine("Ingen spil at redigere.");
        Pause();
        return;
    }

    PrintSpilListe(spilListe);

    Console.Write("Indtast ID på spil der skal redigeres: ");
    var redigerInput = Console.ReadLine();
    if (redigerInput.ToUpper() == "A")
    {
        GoBack();
        return;
    }

    if (int.TryParse(redigerInput, out int id))
    {
        Spil valgtSpil = spilListe.Find(s => s.Id == id);

        if (valgtSpil != null)
        {

            Console.WriteLine();
            Console.WriteLine("Tryk Enter hvis du vil beholde den nuværende værdi.");

            Console.Write($"Ny titel ({valgtSpil.Titel}): ");

            string nyTitel = Console.ReadLine();
            if (nyTitel.ToUpper() == "A")
            {
                GoBack();
                return;
            }
            if (!string.IsNullOrWhiteSpace(nyTitel))
            {
                valgtSpil.Titel = nyTitel;
            }

            Console.WriteLine($"Nuværende genre: {valgtSpil.Genre}");
            Console.WriteLine("Vælg ny genre:");
            Console.WriteLine("1 = Strategi");
            Console.WriteLine("2 = Familie");
            Console.WriteLine("3 = Kortspil");
            Console.WriteLine("4 = Quiz");
            Console.WriteLine("5 = Samarbejde");
            Console.Write("Ny genre (tryk Enter for at beholde): ");
            string genreInput = Console.ReadLine();
            if (genreInput.ToUpper() == "A")
            {
                GoBack();
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

            Console.WriteLine($"Nuværende stand: {valgtSpil.Stand}");
            Console.WriteLine("Vælg ny stand:");
            Console.WriteLine("1 = Ny");
            Console.WriteLine("2 = God");
            Console.WriteLine("3 = Slidt");
            Console.Write("Ny stand (tryk Enter for at beholde): ");
            string standInput = Console.ReadLine();
                        if (standInput.ToUpper() == "A")
            {
                GoBack();
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

            Console.Write($"Ny pris ({valgtSpil.Pris} kr): ");
            string prisInput = Console.ReadLine();
                        if (prisInput.ToUpper() == "A")
            {
                                GoBack();
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

            Console.WriteLine();
            Console.WriteLine("Spillet er blevet opdateret.");
            File.Delete(filsti); // Slet den gamle fil for at undgå duplikater
            GemTilFil(filsti, spilListe); // Gem ændringerne til filen
        }
        else
        {
            Console.WriteLine("Ugyldigt nummer.");
        }
    }
    else
    {
        Console.WriteLine("Du skal skrive et tal.");
    }

    Pause();
}
static void SøgEfterSpilMenu(string filsti, List<Spil> spilListe)
{
    Console.Clear();
    Console.WriteLine("=== Vælg hvad du vil søge efter ===");
    Console.WriteLine("1. Navn");
    Console.WriteLine("2. Genre");
    Console.WriteLine("3. Pris");
    Console.WriteLine("4. Stand");
    Console.WriteLine("5. Søg på flere kriterier");
    Console.WriteLine("6. Gå tilbage");
    Console.Write("> ");
    string svalg = Console.ReadLine();

    switch (svalg)
    {
        case "1":
            SøgEfterSpil(spilListe, filsti);
            break;
        case "2":
            SøgEfterSpil(spilListe, filsti, "Genre");
            break;
        case "3":
            SøgEfterSpil(spilListe, filsti, "Pris");
            break;
        case "4":
            SøgEfterSpil(spilListe, filsti, "Stand");
            break;
        case "5":
            SøgEfterSpil(spilListe, filsti, "Flere");
            break;
        case "6":
            Hovedmenu(filsti, spilListe);
            break;
        default:
            Console.WriteLine("Ugyldigt input. Prøv igen.");
            SøgEfterSpilMenu(filsti, spilListe);
            break;
    }
}

// Søger efter spil ud fra titel
static void SøgEfterSpil(List<Spil> spilListe, string filsti, string svalue = "Titel")
{
    List<Spil> fundneSpil = new List<Spil>();
    string søgning;
    if (spilListe.Count == 0)
    {
        Console.WriteLine("Ingen spil at søge efter.");
        Pause();
        return;
    }
    Console.Clear();
    switch (svalue)
    {
        case "Titel":
            Console.WriteLine("=== Søg efter navn ===");
            Console.Write("Indtast titel eller en del af titlen: ");
            break;
        case "Genre":
            Console.WriteLine("=== Søg efter genre ===");
            Console.Write("Indtast genre: ");
            break;
        case "Pris":
            Console.WriteLine("=== Søg efter pris ===");
            Console.Write("Indtast pris: ");
            break;
        case "Stand":
            Console.WriteLine("=== Søg efter stand ===");
            Console.Write("Indtast stand: ");
            break;
        case "Flere":
            Console.WriteLine("Denne funtkionalitet er ikke implementeret endnu. Søger kun på titel som standard.");
            Console.Write("Indtast søgekriterier: ");
            break;
        default:
            Console.WriteLine("=== Søg efter navn ===");
            Console.Write("Indtast titel eller en del af titlen: ");
            break;
    }
    søgning = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(søgning))
    {
        Console.WriteLine("Du skal skrive noget for at søge.");
        Pause();
        return;
    }

    foreach (Spil spil in spilListe)
    {
        if (svalue == "Titel")
        {
            if (spil.Titel.ToLower().Contains(søgning.ToLower()))
            {
                fundneSpil.Add(spil);
            }
        }
        else if (svalue == "Genre")
        {
            if (spil.Genre.ToString().ToLower().Contains(søgning.ToLower()))
            {
                fundneSpil.Add(spil);
            }
        }
        else if (svalue == "Pris")
        {
            if (spil.Pris.ToString().Contains(søgning))
            {
                fundneSpil.Add(spil);
            }
        }
        else if (svalue == "Stand")
        {
            if (spil.Stand.ToString().ToLower().Contains(søgning.ToLower()))
            {
                fundneSpil.Add(spil);
            }
        }
        else if (svalue == "Flere")
        {
            // Denne del kan udvides til at søge på flere kriterier samtidig, men for nu søger vi bare på titel som standard.
            if (spil.Titel.ToLower().Contains(søgning.ToLower()))
            {
                fundneSpil.Add(spil);
            }

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
        VisAlleSpil(fundneSpil, filsti);
    }

    Pause();
}

// Pause så brugeren kan læse teksten
static void Pause()
{
    Console.WriteLine();
    Console.WriteLine("\nTryk på en tast for at fortsætte...");
    Console.ReadKey();
}
static void Exit()
{     Console.WriteLine("\nTak for at bruge Genspil! Farvel!");
    System.Environment.Exit(0);
}
static void GoBack()
{
    Console.WriteLine("Går tilbage til hovedmenu...");
    Hovedmenu("Datafiler/spil.txt", SpilDataHandler.LæsFraFil("Datafiler/spil.txt"));
}