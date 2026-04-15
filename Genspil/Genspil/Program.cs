using Genspil.Klasser; // Giver adgang til Spil, Genre og Stand
using Genspil.Data;    // Giver adgang til SpilDataHandler

// Sti til filen med spillene
string filsti = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Datafiler\spil.txt");
filsti = Path.GetFullPath(filsti);

Console.WriteLine("Fuld sti: " + filsti);

// Læser spillene fra filen ved opstart
List<Spil> spilListe = SpilDataHandler.LæsFraFil(filsti);

// Starter hovedmenuen
Hovedmenu(filsti, spilListe);

static void Hovedmenu(string filsti, List<Spil> spilListe)
{
    bool kører = true; // Styrer om programmet skal fortsætte

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

        string valg = Console.ReadLine();

        switch (valg)
        {
            case "1":
                VisAlleSpil(spilListe);
                break;

            case "2":
                Spil nytSpil = OpretNytSpil();
                spilListe.Add(nytSpil);

                // Gemmer automatisk efter oprettelse
                SpilDataHandler.GemTilFil(filsti, spilListe);

                Console.WriteLine("Spillet blev tilføjet og gemt.");
                Pause();
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
                Console.WriteLine("Ugyldigt valg. Prøv igen.");
                Pause();
                break;
        }
    }
}

// Viser alle spil
static void VisAlleSpil(List<Spil> spilListe)
{
    Console.Clear();
    Console.WriteLine("=== Liste over spil ===");

    if (spilListe.Count == 0)
    {
        Console.WriteLine("Ingen spil fundet.");
    }
    else
    {
        foreach (Spil spil in spilListe)
        {
            Console.WriteLine(spil.VisInfo());
        }
    }

    Pause();
}

// Opretter et nyt spil
static Spil OpretNytSpil()
{
    Console.Clear();
    Console.WriteLine("=== Opret nyt spil ===");

    Console.Write("Indtast titel: ");
    string titel = Console.ReadLine();

    Console.WriteLine("Vælg genre:");
    Console.WriteLine("1 = Strategi");
    Console.WriteLine("2 = Familie");
    Console.WriteLine("3 = Kortspil");
    Console.WriteLine("4 = Quiz");
    Console.WriteLine("5 = Samarbejde");
    Console.Write("> ");
    int genreValg = int.Parse(Console.ReadLine());
    Genre genre = (Genre)(genreValg - 1); // Trækker 1 fra fordi enum starter ved 0

    Console.WriteLine("Vælg stand:");
    Console.WriteLine("1 = Ny");
    Console.WriteLine("2 = God");
    Console.WriteLine("3 = Slidt");
    Console.Write("> ");
    int standValg = int.Parse(Console.ReadLine());
    Stand stand = (Stand)(standValg - 1); // Trækker 1 fra fordi enum starter ved 0

    Console.Write("Indtast pris: ");
    int pris = int.Parse(Console.ReadLine());

    return new Spil(titel, genre, stand, pris);
}

// Sletter et spil via ID
static void SletSpil(string filsti, List<Spil> spilListe)
{
    Console.Clear();
    Console.WriteLine("=== Slet spil ===");

    if (spilListe.Count == 0)
    {
        Console.WriteLine("Ingen spil at slette.");
        Pause();
        return;
    }

    Console.WriteLine("Spil i systemet:");
    foreach (Spil spil in spilListe)
    {
        Console.WriteLine(spil.VisInfo());
    }

    Console.WriteLine();
    Console.Write("Indtast ID på spil der skal slettes: ");

    if (int.TryParse(Console.ReadLine(), out int id))
    {
        Spil spilDerSkalSlettes = null;

        foreach (Spil spil in spilListe)
        {
            if (spil.Id == id)
            {
                spilDerSkalSlettes = spil;
                break;
            }
        }

        if (spilDerSkalSlettes != null)
        {
            spilListe.Remove(spilDerSkalSlettes);

            // Gemmer automatisk efter sletning
            SpilDataHandler.GemTilFil(filsti, spilListe);

            Console.WriteLine("Spillet er slettet og gemt.");
        }
        else
        {
            Console.WriteLine("Der blev ikke fundet et spil med det ID.");
        }
    }
    else
    {
        Console.WriteLine("Du skal skrive et gyldigt tal.");
    }

    Pause();
}

// Redigerer et spil via ID
static void RedigerSpil(string filsti, List<Spil> spilListe)
{
    Console.Clear();
    Console.WriteLine("=== Rediger spil ===");

    if (spilListe.Count == 0)
    {
        Console.WriteLine("Ingen spil at redigere.");
        Pause();
        return;
    }

    Console.WriteLine("Spil i systemet:");
    foreach (Spil spil in spilListe)
    {
        Console.WriteLine(spil.VisInfo());
    }

    Console.WriteLine();
    Console.Write("Indtast ID på spil der skal redigeres: ");

    if (int.TryParse(Console.ReadLine(), out int id))
    {
        Spil valgtSpil = null;

        foreach (Spil spil in spilListe)
        {
            if (spil.Id == id)
            {
                valgtSpil = spil;
                break;
            }
        }

        if (valgtSpil != null)
        {
            Console.WriteLine();
            Console.WriteLine("Tryk Enter hvis du vil beholde den nuværende værdi.");

            Console.Write($"Ny titel ({valgtSpil.Titel}): ");
            string nyTitel = Console.ReadLine();
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

            // Gemmer automatisk efter redigering
            SpilDataHandler.GemTilFil(filsti, spilListe);

            Console.WriteLine();
            Console.WriteLine("Spillet er blevet opdateret og gemt.");
        }
        else
        {
            Console.WriteLine("Der blev ikke fundet et spil med det ID.");
        }
    }
    else
    {
        Console.WriteLine("Du skal skrive et gyldigt tal.");
    }

    Pause();
}

// Lille menu til søgning
static void SøgEfterSpilMenu(List<Spil> spilListe)
{
    Console.Clear();
    Console.WriteLine("=== Vælg hvad du vil søge efter ===");
    Console.WriteLine("1. Navn");
    Console.WriteLine("2. Genre");
    Console.WriteLine("3. Pris");
    Console.WriteLine("4. Stand");
    Console.WriteLine("5. Gå tilbage");
    Console.Write("> ");
    string svalg = Console.ReadLine();

    switch (svalg)
    {
        case "1":
            SøgEfterSpil(spilListe);
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
            return;

        default:
            Console.WriteLine("Ugyldigt input. Prøv igen.");
            Pause();
            SøgEfterSpilMenu(spilListe);
            break;
    }
}

// Søger efter spil
static void SøgEfterSpil(List<Spil> spilListe, string svalue = "Titel")
{
    List<Spil> fundneSpil = new List<Spil>();

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
        default:
            Console.WriteLine("=== Søg efter navn ===");
            Console.Write("Indtast titel eller en del af titlen: ");
            break;
    }

    string søgning = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(søgning))
    {
        Console.WriteLine("Du skal skrive noget for at søge.");
        Pause();
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
        foreach (Spil spil in fundneSpil)
        {
            Console.WriteLine(spil.VisInfo());
        }
    }

    Pause();
}

// Laver en lille pause i programmet
static void Pause()
{
    Console.WriteLine();
    Console.WriteLine("Tryk på en tast for at fortsætte...");
    Console.ReadKey();
}