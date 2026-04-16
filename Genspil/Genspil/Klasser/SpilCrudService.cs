using Genspil.Data;

namespace Genspil.Klasser // Angiver at denne klasse hører til i projektets namespace Genspil
{
    public static class SpilCrudService // Opretter en statisk serviceklasse til Create, Update og Delete af spil
    {
        public static Spil? OpretNytSpil() // Metode der opretter et nyt spil og returnerer det; kan returnere null hvis brugeren afbryder
        {
            Console.Clear(); // Rydder konsollen så opret-siden starter pænt
            Console.WriteLine("=================================="); // Dekorativ linje
            Console.WriteLine("         OPRET NYT SPIL           "); // Overskrift
            Console.WriteLine("=================================="); // Dekorativ linje
            Console.WriteLine("     Tast A for at afbryde        "); // Hjælpetekst til brugeren
            Console.WriteLine("=================================="); // Dekorativ linje

            Console.Write("Indtast titel: "); // Beder brugeren om at skrive spillets titel
            string titel = (Console.ReadLine() ?? "").Trim(); // Læser input, undgår null og fjerner mellemrum før/efter

            if (titel.ToUpper() == "A") // Tjekker om brugeren vil afbryde
            {
                return null; // Returnerer null for at signalere at oprettelsen blev afbrudt
            }

            Console.WriteLine("Vælg genre:"); // Viser at brugeren nu skal vælge genre
            Console.WriteLine("1 = Strategi"); // Mulighed 1
            Console.WriteLine("2 = Familie"); // Mulighed 2
            Console.WriteLine("3 = Kortspil"); // Mulighed 3
            Console.WriteLine("4 = Quiz"); // Mulighed 4
            Console.WriteLine("5 = Samarbejde"); // Mulighed 5
            Console.Write("Vælg genre: "); // Beder brugeren skrive sit valg

            string genreInput = (Console.ReadLine() ?? "").Trim(); // Læser brugerens input til genre
            if (genreInput.ToUpper() == "A") // Tjekker om brugeren vil afbryde
            {
                return null; // Stopper oprettelsen og returnerer null
            }

            if (!int.TryParse(genreInput, out int genreValg) || genreValg < 1 || genreValg > 5) // Tjekker om input er et gyldigt tal mellem 1 og 5
            {
                Console.WriteLine("Ugyldigt genrevalg."); // Fejlbesked
                ConsoleHelper.Pause(); // Pause så brugeren kan læse beskeden
                return null; // Afslutter metoden uden at oprette et spil
            }

            Genre genre = (Genre)(genreValg - 1); // Laver tallet om til en enum-værdi i Genre

            Console.WriteLine("Vælg stand:"); // Viser at brugeren nu skal vælge stand
            Console.WriteLine("1 = Ny"); // Mulighed 1
            Console.WriteLine("2 = God"); // Mulighed 2
            Console.WriteLine("3 = Slidt"); // Mulighed 3
            Console.Write("Vælg stand: "); // Beder brugeren skrive sit valg

            string standInput = (Console.ReadLine() ?? "").Trim(); // Læser input til stand
            if (standInput.ToUpper() == "A") // Tjekker om brugeren vil afbryde
            {
                return null; // Afbryder oprettelsen
            }

            if (!int.TryParse(standInput, out int standValg) || standValg < 1 || standValg > 3) // Tjekker om input er et gyldigt tal mellem 1 og 3
            {
                Console.WriteLine("Ugyldigt standvalg."); // Fejlbesked
                ConsoleHelper.Pause(); // Pause så brugeren kan læse beskeden
                return null; // Afslutter metoden uden at oprette et spil
            }

            Stand stand = (Stand)(standValg - 1); // Laver tallet om til en enum-værdi i Stand

            Console.Write("Indtast pris: "); // Beder brugeren om at indtaste pris
            string prisInput = (Console.ReadLine() ?? "").Trim(); // Læser input til pris

            if (prisInput.ToUpper() == "A") // Tjekker om brugeren vil afbryde
            {
                return null; // Afbryder oprettelsen
            }

            if (!int.TryParse(prisInput, out int pris)) // Tjekker om prisen er et gyldigt helt tal
            {
                Console.WriteLine("Ugyldig pris."); // Fejlbesked
                ConsoleHelper.Pause(); // Pause så brugeren kan læse beskeden
                return null; // Afslutter metoden uden at oprette et spil
            }

            return new Spil(titel, genre, stand, pris); // Opretter og returnerer et nyt Spil-objekt
        }

        public static void SletSpil(string filsti, List<Spil> spilListe) // Metode der sletter et spil fra listen og gemmer ændringen til fil
        {
            if (spilListe.Count == 0) // Tjekker om listen er tom
            {
                Console.Clear(); // Rydder skærmen
                Console.WriteLine("=== Slet spil ==="); // Overskrift
                Console.WriteLine("Ingen spil at slette."); // Besked til brugeren
                ConsoleHelper.VentPåA(); // Venter på at brugeren skriver A for at gå tilbage
                return; // Afslutter metoden
            }

            char sortering = 'O'; // Standard-sortering sættes til O, som her bruges til oprettelsesrækkefølge/ID

            while (true) // Løkke der holder brugeren i slet-menuen indtil et gyldigt valg eller afbrydelse
            {
                Console.Clear(); // Rydder konsollen
                Console.WriteLine("=== Slet spil ==="); // Overskrift

                List<Spil> sorteretListe = SpilVisningService.HentSorteretListe(spilListe, sortering); // Henter en sorteret kopi af listen
                SpilVisningService.PrintSpilTabel(sorteretListe); // Viser listen i tabelform

                Console.WriteLine("Sortér efter: [N]avn | [G]enre | [S]tand | [P]ris | [O]prettelsesdato | [A]fbryd"); // Viser sorteringsmuligheder
                Console.Write("Indtast ID på spil der skal slettes: "); // Beder brugeren om ID eller sorteringsbogstav

                string input = (Console.ReadLine() ?? "").Trim(); // Læser brugerens input

                if (input.ToUpper() == "A") // Hvis brugeren skriver A
                {
                    return; // Går tilbage til hovedmenuen
                }

                if (input.Length == 1 && "NGSPO".Contains(char.ToUpper(input[0]))) // Hvis input er ét bogstav og er en gyldig sortering
                {
                    sortering = char.ToUpper(input[0]); // Gemmer ny sortering
                    continue; // Starter løkken forfra og viser listen sorteret på ny
                }

                if (int.TryParse(input, out int id)) // Hvis input kan laves om til et ID-tal
                {
                    Spil? spilDerSkalSlettes = spilListe.Find(s => s.Id == id); // Finder spillet med det angivne ID

                    if (spilDerSkalSlettes != null) // Hvis der blev fundet et spil
                    {
                        spilListe.Remove(spilDerSkalSlettes); // Fjerner spillet fra listen
                        SpilDataHandler.GemTilFil(filsti, spilListe); // Gemmer den opdaterede liste til fil

                        Console.WriteLine(); // Tom linje
                        Console.WriteLine("Spillet er slettet og gemt."); // Bekræftelse til brugeren
                    }
                    else // Hvis der ikke findes et spil med det ID
                    {
                        Console.WriteLine(); // Tom linje
                        Console.WriteLine("Ingen spil fundet med det ID."); // Fejlbesked
                    }

                    ConsoleHelper.VentPåA(); // Venter på A før der vendes tilbage
                    return; // Afslutter metoden
                }
                else // Hvis input hverken er et ID eller et gyldigt sorteringsbogstav
                {
                    Console.WriteLine(); // Tom linje
                    Console.WriteLine("Du skal skrive et gyldigt ID eller et sorteringsbogstav."); // Fejlbesked
                    ConsoleHelper.Pause(); // Pause så brugeren kan læse beskeden
                }
            }
        }

        public static void RedigerSpil(string filsti, List<Spil> spilListe) // Metode der redigerer et spil og gemmer ændringerne til fil
        {
            if (spilListe.Count == 0) // Tjekker om listen er tom
            {
                Console.Clear(); // Rydder skærmen
                Console.WriteLine("=== Rediger spil ==="); // Overskrift
                Console.WriteLine("Ingen spil at redigere"); // Besked til brugeren
                ConsoleHelper.VentPåA(); // Venter på A for at gå tilbage
                return; // Afslutter metoden
            }

            char sortering = 'O'; // Standard-sortering sættes til oprettelsesrækkefølge/ID

            while (true) // Holder brugeren i rediger-menuen indtil et gyldigt valg eller afbrydelse
            {
                Console.Clear(); // Rydder konsollen
                Console.WriteLine("=== Rediger spil ==="); // Overskrift

                List<Spil> sorteretListe = SpilVisningService.HentSorteretListe(spilListe, sortering); // Henter en sorteret kopi af listen
                SpilVisningService.PrintSpilTabel(sorteretListe); // Viser den sorterede liste i tabelform

                Console.WriteLine("Sortér efter: [N]avn | [G]enre | [S]tand | [P]ris | [O]prettelsesdato | [A]fbryd"); // Viser sorteringsmuligheder
                Console.Write("Indtast ID på spil der skal redigeres: "); // Beder brugeren skrive ID eller sorteringsbogstav

                string input = (Console.ReadLine() ?? "").Trim(); // Læser input

                if (input.ToUpper() == "A") // Hvis brugeren skriver A
                {
                    return; // Afbryder og går tilbage
                }

                if (input.Length == 1 && "NGSPO".Contains(char.ToUpper(input[0]))) // Hvis input er et gyldigt sorteringsbogstav
                {
                    sortering = char.ToUpper(input[0]); // Opdaterer sorteringen
                    continue; // Starter løkken igen og viser ny sortering
                }

                if (int.TryParse(input, out int id)) // Hvis input kan laves om til et tal
                {
                    Spil? valgtSpil = spilListe.Find(s => s.Id == id); // Finder spillet med det valgte ID

                    if (valgtSpil == null) // Hvis der ikke blev fundet et spil
                    {
                        Console.WriteLine(); // Tom linje
                        Console.WriteLine("Ingen spil fundet med det ID."); // Fejlbesked
                        ConsoleHelper.VentPåA(); // Venter på A
                        return; // Afslutter metoden
                    }

                    Console.WriteLine(); // Tom linje
                    Console.WriteLine("Tryk Enter hvis du vil beholde den nuværende værdi"); // Forklarer at Enter beholder gammel værdi
                    Console.WriteLine("Tast A for at afbryde"); // Forklarer at A afbryder
                    Console.WriteLine("=================================="); // Dekorativ linje

                    Console.Write($"Ny titel ({valgtSpil.Titel}): "); // Viser nuværende titel og beder om ny
                    string nyTitel = (Console.ReadLine() ?? "").Trim(); // Læser ny titel

                    if (nyTitel.ToUpper() == "A") // Hvis brugeren vil afbryde
                    {
                        return; // Går tilbage uden at gemme
                    }

                    if (!string.IsNullOrWhiteSpace(nyTitel)) // Hvis brugeren faktisk har skrevet noget
                    {
                        valgtSpil.Titel = nyTitel; // Opdaterer titlen
                    }

                    Console.WriteLine(); // Tom linje
                    Console.WriteLine($"Nuværende genre: {valgtSpil.Genre}"); // Viser den nuværende genre
                    Console.WriteLine("1 = Strategi"); // Genrevalg 1
                    Console.WriteLine("2 = Familie"); // Genrevalg 2
                    Console.WriteLine("3 = Kortspil"); // Genrevalg 3
                    Console.WriteLine("4 = Quiz"); // Genrevalg 4
                    Console.WriteLine("5 = Samarbejde"); // Genrevalg 5
                    Console.Write("Vælg ny genre (tryk Enter for at beholde): "); // Beder om ny genre eller Enter for at beholde
                    string genreInput = (Console.ReadLine() ?? "").Trim(); // Læser input til genre

                    if (genreInput.ToUpper() == "A") // Hvis brugeren vil afbryde
                    {
                        return; // Går tilbage uden at gemme
                    }

                    if (!string.IsNullOrWhiteSpace(genreInput)) // Hvis brugeren skrev noget
                    {
                        if (int.TryParse(genreInput, out int genreValg)) // Tjekker om input er et tal
                        {
                            if (genreValg >= 1 && genreValg <= 5) // Tjekker om tallet er inden for gyldigt interval
                            {
                                valgtSpil.Genre = (Genre)(genreValg - 1); // Opdaterer genre
                            }
                            else
                            {
                                Console.WriteLine("Ugyldigt genrevalg. Den gamle genre beholdes"); // Fejlbesked
                            }
                        }
                        else
                        {
                            Console.WriteLine("Ugyldigt input. Den gamle genre beholdes"); // Fejlbesked
                        }
                    }

                    Console.WriteLine(); // Tom linje
                    Console.WriteLine($"Nuværende stand: {valgtSpil.Stand}"); // Viser nuværende stand
                    Console.WriteLine("1 = Ny"); // Standvalg 1
                    Console.WriteLine("2 = God"); // Standvalg 2
                    Console.WriteLine("3 = Slidt"); // Standvalg 3
                    Console.Write("Vælg ny stand (tryk Enter for at beholde): "); // Beder om ny stand eller Enter for at beholde
                    string standInput = (Console.ReadLine() ?? "").Trim(); // Læser input til stand

                    if (standInput.ToUpper() == "A") // Hvis brugeren vil afbryde
                    {
                        return; // Går tilbage uden at gemme
                    }

                    if (!string.IsNullOrWhiteSpace(standInput)) // Hvis brugeren skrev noget
                    {
                        if (int.TryParse(standInput, out int standValg)) // Tjekker om input er et tal
                        {
                            if (standValg >= 1 && standValg <= 3) // Tjekker om tallet er gyldigt
                            {
                                valgtSpil.Stand = (Stand)(standValg - 1); // Opdaterer stand
                            }
                            else
                            {
                                Console.WriteLine("Ugyldigt standvalg. Den gamle stand beholdes"); // Fejlbesked
                            }
                        }
                        else
                        {
                            Console.WriteLine("Ugyldigt input. Den gamle stand beholdes"); // Fejlbesked
                        }
                    }

                    Console.WriteLine(); // Tom linje
                    Console.Write($"Ny pris ({valgtSpil.Pris} kr): "); // Viser nuværende pris og beder om ny
                    string prisInput = (Console.ReadLine() ?? "").Trim(); // Læser input til pris

                    if (prisInput.ToUpper() == "A") // Hvis brugeren vil afbryde
                    {
                        return; // Går tilbage uden at gemme
                    }

                    if (!string.IsNullOrWhiteSpace(prisInput)) // Hvis brugeren skrev noget
                    {
                        if (int.TryParse(prisInput, out int nyPris)) // Tjekker om input er et gyldigt tal
                        {
                            valgtSpil.Pris = nyPris; // Opdaterer prisen
                        }
                        else
                        {
                            Console.WriteLine("Ugyldig pris. Den gamle pris beholdes"); // Fejlbesked
                        }
                    }

                    SpilDataHandler.GemTilFil(filsti, spilListe); // Gemmer hele listen med ændringer til filen

                    Console.WriteLine(); // Tom linje
                    Console.WriteLine("Spillet er blevet opdateret og gemt"); // Bekræftelse til brugeren
                    ConsoleHelper.VentPåA(); // Venter på A før der gås tilbage
                    return; // Afslutter metoden
                }
                else // Hvis input ikke er gyldigt
                {
                    Console.WriteLine(); // Tom linje
                    Console.WriteLine("Du skal skrive et gyldigt ID eller et sorteringsbogstav"); // Fejlbesked
                    ConsoleHelper.Pause(); // Pause så brugeren kan læse beskeden
                }
            }
        }
    }
}