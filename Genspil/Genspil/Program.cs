using Genspil.Klasser;
using Genspil.Data;

// Sti til tekstfilen
string filsti = "Datafiler/spil.txt";

// Læser spillene fra filen når programmet starter
List<Spil> spilListe = SpilDataHandler.LæsFraFil(filsti);

// Styrer om programmet skal fortsætte
bool kører = true;

// Menu-loop
while (kører)
{
    Console.Clear();
    Console.WriteLine("=== Genspil Menu ===");
    Console.WriteLine("1. Vis alle spil");
    Console.WriteLine("2. Tilføj nyt spil");
    Console.WriteLine("3. Gem spil til fil");
    Console.WriteLine("4. Slet spil");
    Console.WriteLine("5. Afslut");
    Console.Write("Vælg en mulighed: ");

    string valg = Console.ReadLine();

    switch (valg)
    {
        case "1":
            // Viser alle spil
            VisAlleSpil(spilListe);
            break;

        case "2":
            // Opretter et nyt spil
            Spil nytSpil = OpretNytSpil();
            spilListe.Add(nytSpil);

            Console.WriteLine("Spillet blev tilføjet.");
            Pause();
            break;

        case "3":
            // Gemmer listen til fil
            SpilDataHandler.GemTilFil(filsti, spilListe);

            Console.WriteLine("Spillene er gemt til fil.");
            Pause();
            break;

        case "4":
            // Sletter et spil
            SletSpil(spilListe);
            break;

        case "5":
            // Afslutter programmet
            kører = false;
            break;

        default:
            Console.WriteLine("Ugyldigt valg. Prøv igen.");
            Pause();
            break;
    }
}

// Viser alle spil i listen
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
        for (int i = 0; i < spilListe.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {spilListe[i].VisInfo()}");
        }
    }

    Pause();
}

// Opretter et nyt spil ud fra brugerens input
static Spil OpretNytSpil()
{
    Console.Clear();
    Console.WriteLine("=== Opret nyt spil ===");

    Console.Write("Indtast titel: ");
    string titel = Console.ReadLine();

    Console.WriteLine("Vælg genre:");
    Console.WriteLine("0 = Strategi");
    Console.WriteLine("1 = Familie");
    Console.WriteLine("2 = Kortspil");
    Console.WriteLine("3 = Quiz");
    Console.WriteLine("4 = Samarbejde");
    int genreValg = int.Parse(Console.ReadLine());
    Genre genre = (Genre)genreValg;

    Console.WriteLine("Vælg stand:");
    Console.WriteLine("0 = Ny");
    Console.WriteLine("1 = God");
    Console.WriteLine("2 = Slidt");
    int standValg = int.Parse(Console.ReadLine());
    Stand stand = (Stand)standValg;

    Console.Write("Indtast pris: ");
    int pris = int.Parse(Console.ReadLine());

    return new Spil(titel, genre, stand, pris);
}

// Sletter et spil fra listen
static void SletSpil(List<Spil> spilListe)
{
    Console.Clear();
    Console.WriteLine("=== Slet spil ===");

    if (spilListe.Count == 0)
    {
        Console.WriteLine("Ingen spil at slette.");
        Pause();
        return;
    }

    for (int i = 0; i < spilListe.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {spilListe[i].VisInfo()}");
    }

    Console.Write("Vælg nummer på spil der skal slettes: ");

    if (int.TryParse(Console.ReadLine(), out int valg))
    {
        if (valg > 0 && valg <= spilListe.Count)
        {
            spilListe.RemoveAt(valg - 1);
            Console.WriteLine("Spillet er slettet.");
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

// Pause så brugeren kan læse teksten
static void Pause()
{
    Console.WriteLine();
    Console.WriteLine("Tryk på en tast for at fortsætte...");
    Console.ReadKey();
}