using Genspil.Data;

namespace Genspil.Klasser
{
    public static class MenuService
    {
        // Viser hovedmenuen og sender brugeren videre til de rigtige funktioner
        public static void Hovedmenu(string filsti, List<Spil> spilListe)
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
                Console.WriteLine("6. Vis forespørgsler");
                Console.WriteLine("7. Afslut");
                Console.WriteLine("==================================");
                Console.Write("Vælg en mulighed: ");

                string valg = Console.ReadKey().KeyChar.ToString().Trim();
                Console.WriteLine();

                switch (valg)
                {
                    case "1":
                        SpilVisningService.VisAlleSpil(filsti, spilListe);
                        break;

                    case "2":
                        bool fortsætTilføjelse = true;

                        while (fortsætTilføjelse)
                        {
                            Spil? nytSpil = SpilCrudService.OpretNytSpil(filsti, spilListe);

                            if (nytSpil == null)
                            {
                                fortsætTilføjelse = false;
                                break;
                            }

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
                        SpilCrudService.SletSpil(filsti, spilListe);
                        break;

                    case "4":
                        SpilCrudService.RedigerSpil(filsti, spilListe);
                        break;

                    case "5":
                        SøgningService.SøgEfterSpilMenu(spilListe);
                        break;

                    case "6":
                        SpilVisningService.VisForespørgsler(spilListe);
                        break;

                    case "7":
                        kører = false;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("Ugyldigt valg. Prøv igen.");
                        ConsoleHelper.Pause();
                        break;
                }
            }
        }
    }
}