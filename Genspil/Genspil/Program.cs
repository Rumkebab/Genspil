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

        string valg = Console.ReadLine();

        switch (valg)
        {
            case "1":
                VisAlleSpil(spilListe);
                break;

            case "2":
                bool fortsætTilføjelse = true;

                while (fortsætTilføjelse)
                {
                    Spil nytSpil = OpretNytSpil();

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
                    Console.WriteLine("0. Afbryd og vend tilbage til hovedmenuen");
                    Console.WriteLine("==================================");

                    string tilfoejValg = "";
                    while (tilfoejValg != "1" && tilfoejValg != "0")
                    {
                        Console.Write("Valg: ");
                        tilfoejValg = Console.ReadLine();

                        if (tilfoejValg != "1" && tilfoejValg != "0")
                        {
                            Console.WriteLine("Ugyldigt valg. Indtast 1 eller 0.");
                        }
                    }

                    if (tilfoejValg == "0")
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
                VentPåNul();
                break;
        }
    }
}

// Viser alle spil
static void VisAlleSpil(List<Spil> spilListe)
{
    Console.Clear();
    Console.WriteLine("==================================");
    Console.WriteLine("          LISTE OVER SPIL         ");
    Console.WriteLine("==================================");
    Console.WriteLine("Indtast 0 for at afbryde og vende tilbage til hovedmenuen.");
    Console.WriteLine("==================================");

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

    Console.WriteLine("==================================");
    VentPåNul("Valg: ");
}

// Opretter et nyt spil
static Spil OpretNytSpil()
{
    Console.Clear();
    Console.WriteLine("==================================");
    Console.WriteLine("          OPRET NYT SPIL          ");
    Console.WriteLine("==================================");
    Console.WriteLine("Indtast 0 for at afbryde og vende tilbage til hovedmenuen.");
    Console.WriteLine("==================================");

    Console.Write("Indtast titel: ");
    string titel = Console.ReadLine();

    if (titel == "0")
    {
        return null;
    }

    Console.WriteLine();
    Console.WriteLine("Vælg genre:");
    Console.WriteLine("1. Strategi");
    Console.WriteLine("2. Familie");
    Console.WriteLine("3. Kortspil");
    Console.WriteLine("4. Quiz");
    Console.WriteLine("5. Samarbejde");
    Console.Write("Vælg genre: ");
    string genreInput = Console.ReadLine();

    if (genreInput == "0")
    {
        return null;
    }

    int genreValg = int.Parse(genreInput);
    Genre genre = (Genre)(genreValg - 1);

    Console.WriteLine();
    Console.WriteLine("Vælg stand:");
    Console.WriteLine("1. Ny");
    Console.WriteLine("2. God");
    Console.WriteLine("3. Slidt");
    Console.Write("Vælg stand: ");
    string standInput = Console.ReadLine();

    if (standInput == "0")
    {
        return null;
    }

    int standValg = int.Parse(standInput);
    Stand stand = (Stand)(standValg - 1);

    Console.WriteLine();
    Console.Write("Indtast pris: ");
    string prisInput = Console.ReadLine();

    if (prisInput == "0")
    {
        return null;
    }

    int pris = int.Parse(prisInput);

    return new Spil(titel, genre, stand, pris);
}

// Sletter et spil via ID
static void SletSpil(string filsti, List<Spil> spilListe)
{
    Console.Clear();
    Console.WriteLine("==================================");
    Console.WriteLine("            SLET SPIL             ");
    Console.WriteLine("==================================");
    Console.WriteLine("Indtast 0 for at afbryde og vende tilbage til hovedmenuen.");
    Console.WriteLine("==================================");

    if (spilListe.Count == 0)
    {
        Console.WriteLine("Ingen spil at slette.");
        Console.WriteLine("==================================");
        VentPåNul();
        return;
    }

    Console.WriteLine("Spil i systemet:");
    foreach (Spil spil in spilListe)
    {
        Console.WriteLine(spil.VisInfo());
    }

    Console.WriteLine("==================================");
    Console.Write("Indtast ID på spil der skal slettes: ");

    if (int.TryParse(Console.ReadLine(), out int id))
    {
        if (id == 0)
        {
            return;
        }

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

            Console.WriteLine();
            Console.WriteLine("Spillet er slettet og gemt.");
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine("Der blev ikke fundet et spil med det ID.");
        }
    }
    else
    {
        Console.WriteLine();
        Console.WriteLine("Du skal skrive et gyldigt tal.");
    }

    VentPåNul();
}

// Redigerer et spil via ID
static void RedigerSpil(string filsti, List<Spil> spilListe)
{
    Console.Clear();
    Console.WriteLine("==================================");
    Console.WriteLine("          REDIGER SPIL            ");
    Console.WriteLine("==================================");
    Console.WriteLine("Indtast 0 for at afbryde og vende tilbage til hovedmenuen.");
    Console.WriteLine("==================================");

    if (spilListe.Count == 0)
    {
        Console.WriteLine("Ingen spil at redigere.");
        Console.WriteLine("==================================");
        VentPåNul();
        return;
    }

    Console.WriteLine("Spil i systemet:");
    foreach (Spil spil in spilListe)
    {
        Console.WriteLine(spil.VisInfo());
    }

    Console.WriteLine("==================================");
    Console.Write("Indtast ID på spil der skal redigeres: ");

    if (int.TryParse(Console.ReadLine(), out int id))
    {
        if (id == 0)
        {
            return;
        }

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
            Console.WriteLine("==================================");

            Console.Write($"Ny titel ({valgtSpil.Titel}): ");
            string nyTitel = Console.ReadLine();

            if (nyTitel == "0")
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(nyTitel))
            {
                valgtSpil.Titel = nyTitel;
            }

            Console.WriteLine();
            Console.WriteLine($"Nuværende genre: {valgtSpil.Genre}");
            Console.WriteLine("1. Strategi");
            Console.WriteLine("2. Familie");
            Console.WriteLine("3. Kortspil");
            Console.WriteLine("4. Quiz");
            Console.WriteLine("5. Samarbejde");
            Console.Write("Vælg ny genre (tryk Enter for at beholde): ");
            string genreInput = Console.ReadLine();

            if (genreInput == "0")
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
            Console.WriteLine("1. Ny");
            Console.WriteLine("2. God");
            Console.WriteLine("3. Slidt");
            Console.Write("Vælg ny stand (tryk Enter for at beholde): ");
            string standInput = Console.ReadLine();

            if (standInput == "0")
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
            string prisInput = Console.ReadLine();

            if (prisInput == "0")
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

            // Gemmer automatisk efter redigering
            SpilDataHandler.GemTilFil(filsti, spilListe);

            Console.WriteLine();
            Console.WriteLine("Spillet er blevet opdateret og gemt.");
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine("Der blev ikke fundet et spil med det ID.");
        }
    }
    else
    {
        Console.WriteLine();
        Console.WriteLine("Du skal skrive et gyldigt tal.");
    }

    VentPåNul();
}

// Lille menu til søgning
static void SøgEfterSpilMenu(List<Spil> spilListe)
{
    Console.Clear();
    Console.WriteLine("==================================");
    Console.WriteLine("          SØG EFTER SPIL          ");
    Console.WriteLine("==================================");
    Console.WriteLine("Indtast 0 for at afbryde og vende tilbage til hovedmenuen.");
    Console.WriteLine("==================================");
    Console.WriteLine("1. Navn");
    Console.WriteLine("2. Genre");
    Console.WriteLine("3. Pris");
    Console.WriteLine("4. Stand");
    Console.WriteLine("==================================");
    Console.Write("Vælg en mulighed: ");

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
        case "0":
            return;

        default:
            Console.WriteLine();
            Console.WriteLine("Ugyldigt input. Prøv igen.");
            VentPåNul();
            break;
    }
}

// Søger efter spil
static void SøgEfterSpil(List<Spil> spilListe, string svalue = "Titel")
{
    List<Spil> fundneSpil = new List<Spil>();

    Console.Clear();
    Console.WriteLine("==================================");
    Console.WriteLine("          SØGNING I SPIL          ");
    Console.WriteLine("==================================");
    Console.WriteLine("Indtast 0 for at afbryde og vende tilbage til hovedmenuen.");
    Console.WriteLine("==================================");

    if (spilListe.Count == 0)
    {
        Console.WriteLine("Ingen spil at søge efter.");
        Console.WriteLine("==================================");
        VentPåNul();
        return;
    }

    switch (svalue)
    {
        case "Titel":
            Console.Write("Indtast titel eller en del af titlen: ");
            break;
        case "Genre":
            Console.Write("Indtast genre: ");
            break;
        case "Pris":
            Console.Write("Indtast pris: ");
            break;
        case "Stand":
            Console.Write("Indtast stand: ");
            break;
        default:
            Console.Write("Indtast titel eller en del af titlen: ");
            break;
    }

    string søgning = Console.ReadLine();

    if (søgning == "0")
    {
        return;
    }

    if (string.IsNullOrWhiteSpace(søgning))
    {
        Console.WriteLine();
        Console.WriteLine("Du skal skrive noget for at søge.");
        VentPåNul();
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

    Console.WriteLine("==================================");

    if (fundneSpil.Count == 0)
    {
        Console.WriteLine("Ingen spil fundet.");
    }
    else
    {
        Console.WriteLine("Fundne spil:");
        foreach (Spil spil in spilListe)
        {
            Console.WriteLine(spil.VisInfo());
        }
    }

    Console.WriteLine("==================================");
    VentPåNul();
}

// Venter på at brugeren skriver 0
static void VentPåNul(string promptTekst = "Indtast 0: ")
{
    string input = "";

    while (input != "0")
    {
        Console.Write(promptTekst);
        input = Console.ReadLine();
    }
}