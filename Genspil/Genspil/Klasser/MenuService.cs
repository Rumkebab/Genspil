using Genspil.Data;

namespace Genspil.Klasser // Angiver at denne klasse hører til i projektets namespace Genspil
{
    public static class MenuService // Opretter en statisk serviceklasse, som styrer hovedmenuen i programmet
    {
        public static void Hovedmenu(string filsti, List<Spil> spilListe) // Metode der viser hovedmenuen og håndterer navigation
        {
            bool kører = true; // Variabel der styrer, om menuen skal blive ved med at køre

            while (kører) // Så længe kører er true, bliver hovedmenuen ved med at blive vist
            {
                Console.Clear(); // Rydder konsollen, så menuen vises pænt hver gang
                Console.WriteLine("=================================="); // Udskriver en dekorativ linje
                Console.WriteLine("          GENSPIL MENU            "); // Udskriver overskriften på hovedmenuen
                Console.WriteLine("=================================="); // Udskriver endnu en dekorativ linje
                Console.WriteLine("1. Vis alle spil"); // Menupunkt 1
                Console.WriteLine("2. Tilføj nyt spil"); // Menupunkt 2
                Console.WriteLine("3. Slet spil"); // Menupunkt 3
                Console.WriteLine("4. Rediger spil"); // Menupunkt 4
                Console.WriteLine("5. Søg efter spil"); // Menupunkt 5
                Console.WriteLine("6. Afslut"); // Menupunkt 6
                Console.WriteLine("=================================="); // Udskriver en bundlinje
                Console.Write("Vælg en mulighed: "); // Beder brugeren vælge et punkt i menuen

                string valg = (Console.ReadKey().KeyChar.ToString() ?? "").Trim(); // Læser brugerens input, undgår null og fjerner ekstra mellemrum
                Console.WriteLine(); // Gør så næste output starter på en ny linje
        
                switch (valg) // Kigger på hvad brugeren skrev og vælger handling ud fra det
                {
                    case "1": // Hvis brugeren vælger 1
                        SpilVisningService.VisAlleSpil(spilListe); // Kalder metoden der viser alle spil
                        break; // Afslutter dette case

                    case "2": // Hvis brugeren vælger 2
                        bool fortsætTilføjelse = true; // Variabel der styrer om brugeren vil tilføje flere spil i træk

                        while (fortsætTilføjelse) // Så længe brugeren vil fortsætte med at tilføje spil
                        {
                            Spil? nytSpil = SpilCrudService.OpretNytSpil(filsti, spilListe); // Kalder metoden der opretter et nyt spil og gemmer resultatet i variablen nytSpil

                            if (nytSpil == null) // Hvis metoden returnerer null, betyder det at brugeren afbrød oprettelsen
                            {
                                fortsætTilføjelse = false; // Stopper løkken
                                break; // Hopper ud af while-løkken
                            }

                            Console.WriteLine(); // Skriver en tom linje for luft i layoutet
                            Console.WriteLine("Spillet blev tilføjet og gemt."); // Bekræfter at spillet er gemt
                            Console.WriteLine("=================================="); // Dekorativ linje
                            Console.WriteLine("1. Tilføj endnu et spil"); // Giver mulighed for at oprette endnu et spil
                            Console.WriteLine("A. Afbryd og vend tilbage til hovedmenuen"); // Giver mulighed for at gå tilbage til hovedmenuen
                            Console.WriteLine("=================================="); // Dekorativ linje

                            string tilfoejValg = ""; // Tom variabel som senere bruges til brugerens valg

                            while (tilfoejValg != "1" && tilfoejValg != "A") // Bliver ved indtil brugeren skriver et gyldigt valg
                            {
                                Console.Write("Valg: "); // Beder brugeren om at vælge
                                tilfoejValg = (Console.ReadLine() ?? "").Trim().ToUpper(); // Læser input, fjerner mellemrum og gør det til store bogstaver

                                if (tilfoejValg != "1" && tilfoejValg != "A") // Hvis input ikke er gyldigt
                                {
                                    Console.WriteLine("Ugyldigt valg. Indtast 1 eller A."); // Viser fejlbesked
                                }
                            }

                            if (tilfoejValg == "A") // Hvis brugeren skriver A
                            {
                                fortsætTilføjelse = false; // Stopper tilføjelses-løkken og vender tilbage til hovedmenuen
                            }
                        }
                        break; // Afslutter case 2

                    case "3": // Hvis brugeren vælger 3
                        SpilCrudService.SletSpil(filsti, spilListe); // Kalder metoden der sletter et spil
                        break; // Afslutter case 3

                    case "4": // Hvis brugeren vælger 4
                        SpilCrudService.RedigerSpil(filsti, spilListe); // Kalder metoden der redigerer et spil
                        break; // Afslutter case 4

                    case "5": // Hvis brugeren vælger 5
                        SøgningService.SøgEfterSpilMenu(spilListe); // Kalder søgemenuen
                        break; // Afslutter case 5

                    case "6": // Hvis brugeren vælger 6
                        kører = false; // Sætter kører til false, så while-løkken stopper og programmet afsluttes
                        break; // Afslutter case 6

                    default: // Hvis brugeren skriver noget andet end 1-6
                        Console.WriteLine(); // Tom linje
                        Console.WriteLine("Ugyldigt valg. Prøv igen."); // Fejlbesked til brugeren
                        ConsoleHelper.Pause(); // Giver brugeren tid til at læse beskeden før menuen vises igen
                        break; // Afslutter default
                }
            }
        }
    }
}