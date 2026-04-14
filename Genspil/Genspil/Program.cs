using Genspil.Klasser; // Giver adgang til Spil, Genre og Stand
using Genspil.Data;    // Giver adgang til SpilDataHandler (filhåndtering)

// Sti til vores tekstfil hvor spillene bliver gemt
string filsti = "Datafiler/spil.txt";

// Læser alle spil fra filen og gemmer dem i en liste
List<Spil> spilListe = SpilDataHandler.LæsFraFil(filsti);

// Variabel der styrer om programmet skal fortsætte med at køre
bool kører = true;

// Så længe kører er true, bliver menuen ved med at blive vist
while (kører)
{
    Console.Clear(); // Rydder skærmen så det ser pænt ud

    // Viser menuen til brugeren
    Console.WriteLine("=== Genspil Menu ===");
    Console.WriteLine("1. Vis alle spil");
    Console.WriteLine("2. Tilføj nyt spil");
    Console.WriteLine("3. Gem spil til fil");
    Console.WriteLine("4. Afslut");
    Console.Write("Vælg en mulighed: ");

    // Læser brugerens valg
    string valg = Console.ReadLine();

    // Switch bruges til at vælge hvad der skal ske ud fra brugerens input
    switch (valg)
    {
        case "1":
            // Viser alle spil i listen
            VisAlleSpil(spilListe);
            break;

        case "2":
            // Opretter et nyt spil via metode
            Spil nytSpil = OpretNytSpil();

            // Tilføjer spillet til listen
            spilListe.Add(nytSpil);

            Console.WriteLine("Spillet blev tilføjet.");
            Pause(); // Venter så brugeren kan nå at læse beskeden
            break;

        case "3":
            // Gemmer alle spil fra listen til filen
            SpilDataHandler.GemTilFil(filsti, spilListe);

            Console.WriteLine("Spillene er gemt til fil.");
            Pause();
            break;

        case "4":
            // Sletter et spil fra listen
            SletSpil(spilListe);
            break;

        case "5":
            //stopper programmet ved at sætte til false
            kører = false;
            break;

        default:
            // Hvis brugeren skriver noget forkert
            Console.WriteLine("Ugyldigt valg. Prøv igen.");
            Pause();
            break;
    }
}

// Metode til at vise alle spil i listen
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

// Metode til at oprette et nyt spil via brugerinput
static Spil OpretNytSpil()
{
    Console.Clear();
    Console.WriteLine("=== Opret nyt spil ===");

    // Brugeren indtaster titel
    Console.Write("Indtast titel: ");
    string titel = Console.ReadLine();

    // Brugeren vælger genre
    Console.WriteLine("Vælg genre:");
    Console.WriteLine("0 = Strategi");
    Console.WriteLine("1 = Familie");
    Console.WriteLine("2 = Kortspil");
    Console.WriteLine("3 = Quiz");
    Console.WriteLine("4 = Samarbejde");

    // Læser valg og laver det om til enum
    int genreValg = int.Parse(Console.ReadLine());
    Genre genre = (Genre)genreValg;

    // Brugeren vælger stand
    Console.WriteLine("Vælg stand:");
    Console.WriteLine("0 = Ny");
    Console.WriteLine("1 = God");
    Console.WriteLine("2 = Slidt");

    int standValg = int.Parse(Console.ReadLine());
    Stand stand = (Stand)standValg;

    // Brugeren indtaster pris
    Console.Write("Indtast pris: ");
    int pris = int.Parse(Console.ReadLine());

    // Returnerer et nyt Spil objekt med de indtastede værdier
    return new Spil(titel, genre, stand, pris);
}


static void Pause()
{
    Console.WriteLine();
    Console.WriteLine("Tryk på en tast for at fortsætte...");
    Console.ReadKey(); // Venter på at brugeren trykker en tast
}