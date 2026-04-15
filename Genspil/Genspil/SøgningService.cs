using Genspil.Klasser; // Giver adgang til model-klassen Spil

namespace Genspil // Angiver at denne klasse hører til i projektets namespace Genspil
{
    public static class SøgningService // Opretter en statisk serviceklasse til alt der handler om søgning
    {
        public static void SøgEfterSpilMenu(List<Spil> spilListe) // Viser søgemenuen og lader brugeren vælge hvilken type søgning der skal bruges
        {
            Console.Clear(); // Rydder konsollen så søgemenuen vises pænt
            Console.WriteLine("=================================="); // Dekorativ linje
            Console.WriteLine("    VÆLG HVAD DU VIL SØGE EFTER   "); // Overskrift til søgemenuen
            Console.WriteLine("=================================="); // Dekorativ linje
            Console.WriteLine("1. Titel"); // Mulighed for at søge efter titel
            Console.WriteLine("2. Genre"); // Mulighed for at søge efter genre
            Console.WriteLine("3. Pris"); // Mulighed for at søge efter pris
            Console.WriteLine("4. Stand"); // Mulighed for at søge efter stand
            Console.WriteLine("5. Søg på flere kriterier"); // Mulighed for at søge på flere felter samtidig
            Console.WriteLine("6. Afbryd og vend tilbage til hovedmenuen"); // Mulighed for at afslutte søgemenuen
            Console.WriteLine("=================================="); // Dekorativ linje
            Console.Write("Valg: "); // Beder brugeren om at vælge et menupunkt

            string svalg = (Console.ReadLine() ?? "").Trim(); // Læser brugerens valg, undgår null og fjerner mellemrum

            switch (svalg) // Kigger på brugerens valg
            {
                case "1": // Hvis brugeren vælger 1
                    SøgEfterSpil(spilListe, "Titel"); // Starter søgning efter titel
                    break; // Afslutter case

                case "2": // Hvis brugeren vælger 2
                    SøgEfterSpil(spilListe, "Genre"); // Starter søgning efter genre
                    break; // Afslutter case

                case "3": // Hvis brugeren vælger 3
                    SøgEfterSpil(spilListe, "Pris"); // Starter søgning efter pris
                    break; // Afslutter case

                case "4": // Hvis brugeren vælger 4
                    SøgEfterSpil(spilListe, "Stand"); // Starter søgning efter stand
                    break; // Afslutter case

                case "5": // Hvis brugeren vælger 5
                    SøgEfterFlereKriterier(spilListe); // Starter søgning med flere kriterier
                    break; // Afslutter case

                case "6": // Hvis brugeren vælger 6
                    return; // Går tilbage til hovedmenuen ved at afslutte metoden

                default: // Hvis brugeren skriver noget ugyldigt
                    Console.WriteLine(); // Tom linje for luft
                    Console.WriteLine("Ugyldigt input. Prøv igen."); // Fejlbesked
                    ConsoleHelper.Pause(); // Giver brugeren tid til at læse beskeden
                    break; // Afslutter default
            }
        }

        public static void SøgEfterSpil(List<Spil> spilListe, string svalue = "Titel") // Søger i listen ud fra ét valgt kriterium, fx titel, genre, pris eller stand
        {
            List<Spil> fundneSpil = new List<Spil>(); // Opretter en tom liste til de spil der matcher søgningen

            if (spilListe.Count == 0) // Tjekker om der overhovedet er nogen spil at søge i
            {
                Console.WriteLine("Ingen spil at søge efter."); // Besked til brugeren
                ConsoleHelper.VentPåA(); // Venter på A før der gås tilbage
                return; // Afslutter metoden
            }

            Console.Clear(); // Rydder konsollen før søgesiden vises
            Console.WriteLine("=================================="); // Dekorativ linje

            switch (svalue) // Kigger på hvilket søgekriterium der er valgt
            {
                case "Titel": // Hvis der søges efter titel
                    Console.WriteLine("         SØG EFTER TITEL          "); // Overskrift
                    break; // Afslutter case

                case "Genre": // Hvis der søges efter genre
                    Console.WriteLine("         SØG EFTER GENRE          "); // Overskrift
                    break; // Afslutter case

                case "Pris": // Hvis der søges efter pris
                    Console.WriteLine("          SØG EFTER PRIS          "); // Overskrift
                    break; // Afslutter case

                case "Stand": // Hvis der søges efter stand
                    Console.WriteLine("         SØG EFTER STAND          "); // Overskrift
                    break; // Afslutter case

                default: // Standard hvis intet andet matcher
                    Console.WriteLine("         SØG EFTER TITEL          "); // Bruger titel som standard
                    break; // Afslutter default
            }

            Console.WriteLine("=================================="); // Dekorativ linje
            Console.WriteLine("Tast B for at gå tilbage til søgemenuen."); // Forklarer hvordan man går tilbage til forrige side
            Console.WriteLine("Tast A for at vende tilbage til hovedmenuen."); // Forklarer hvordan man går direkte tilbage til hovedmenuen
            Console.WriteLine("=================================="); // Dekorativ linje

            switch (svalue) // Viser den rigtige input-prompt ud fra det valgte søgekriterium
            {
                case "Titel": // Hvis der søges efter titel
                    Console.Write("Indtast titel eller en del af titlen: "); // Beder om titel eller del af titel
                    break; // Afslutter case

                case "Genre": // Hvis der søges efter genre
                    Console.Write("Indtast genre: "); // Beder om genre
                    break; // Afslutter case

                case "Pris": // Hvis der søges efter pris
                    Console.Write("Indtast pris: "); // Beder om pris
                    break; // Afslutter case

                case "Stand": // Hvis der søges efter stand
                    Console.Write("Indtast stand: "); // Beder om stand
                    break; // Afslutter case

                default: // Standard hvis intet andet matcher
                    Console.Write("Indtast titel eller en del af titlen: "); // Standard-prompt
                    break; // Afslutter default
            }

            string søgning = (Console.ReadLine() ?? "").Trim(); // Læser brugerens søgetekst

            if (søgning.ToUpper() == "A") // Hvis brugeren vil gå direkte tilbage til hovedmenuen
            {
                return; // Afslutter metoden
            }

            if (søgning.ToUpper() == "B") // Hvis brugeren vil tilbage til søgemenuen
            {
                SøgEfterSpilMenu(spilListe); // Kalder søgemenuen igen
                return; // Afslutter nuværende metode
            }

            if (string.IsNullOrWhiteSpace(søgning)) // Hvis brugeren ikke skrev noget
            {
                Console.WriteLine("Du skal skrive noget for at søge."); // Fejlbesked
                VentPåSøgningValg(spilListe); // Giver valg mellem søgemenu og hovedmenu
                return; // Afslutter metoden
            }

            foreach (Spil spil in spilListe) // Går igennem hvert spil i listen
            {
                if (svalue == "Titel" && spil.Titel.ToLower().Contains(søgning.ToLower())) // Hvis der søges efter titel og titlen matcher helt eller delvist
                {
                    fundneSpil.Add(spil); // Tilføjer spillet til listen over fundne spil
                }
                else if (svalue == "Genre" && spil.Genre.ToString().ToLower().Contains(søgning.ToLower())) // Hvis der søges efter genre og genren matcher
                {
                    fundneSpil.Add(spil); // Tilføjer spillet til listen over fundne spil
                }
                else if (svalue == "Pris" && spil.Pris.ToString().Contains(søgning)) // Hvis der søges efter pris og prisen matcher
                {
                    fundneSpil.Add(spil); // Tilføjer spillet til listen over fundne spil
                }
                else if (svalue == "Stand" && spil.Stand.ToString().ToLower().Contains(søgning.ToLower())) // Hvis der søges efter stand og standen matcher
                {
                    fundneSpil.Add(spil); // Tilføjer spillet til listen over fundne spil
                }
            }

            Console.WriteLine(); // Tom linje for luft

            if (fundneSpil.Count == 0) // Hvis ingen spil blev fundet
            {
                Console.WriteLine("Ingen spil fundet."); // Besked til brugeren
            }
            else // Hvis der blev fundet et eller flere spil
            {
                Console.WriteLine("Fundne spil:"); // Overskrift til resultaterne
                SpilVisningService.PrintSpilTabel(fundneSpil); // Viser de fundne spil i tabel-format
            }

            VentPåSøgningValg(spilListe); // Når søgningen er færdig, får brugeren valg om at gå til søgemenu eller hovedmenu
        }

        public static void SøgEfterFlereKriterier(List<Spil> spilListe) // Søger efter spil ud fra flere kriterier samtidig
        {
            Console.Clear(); // Rydder konsollen
            Console.WriteLine("=================================="); // Dekorativ linje
            Console.WriteLine("      SØG PÅ FLERE KRITERIER      "); // Overskrift
            Console.WriteLine("=================================="); // Dekorativ linje
            Console.WriteLine("Tast B for at gå tilbage til søgemenuen."); // Hjælpetekst
            Console.WriteLine("Tast A for at vende tilbage til hovedmenuen."); // Hjælpetekst
            Console.WriteLine("=================================="); // Dekorativ linje

            Console.Write("Titel (tryk Enter for at springe over): "); // Beder om titel, men det er frivilligt
            string titel = (Console.ReadLine() ?? "").Trim(); // Læser input
            if (titel.ToUpper() == "A") return; // Går til hovedmenu hvis brugeren skriver A
            if (titel.ToUpper() == "B") // Går tilbage til søgemenu hvis brugeren skriver B
            {
                SøgEfterSpilMenu(spilListe); // Kalder søgemenuen
                return; // Afslutter metoden
            }

            Console.Write("Genre (tryk Enter for at springe over): "); // Beder om genre
            string genre = (Console.ReadLine() ?? "").Trim(); // Læser input
            if (genre.ToUpper() == "A") return; // Går til hovedmenu hvis A
            if (genre.ToUpper() == "B") // Går til søgemenu hvis B
            {
                SøgEfterSpilMenu(spilListe); // Kalder søgemenuen
                return; // Afslutter metoden
            }

            Console.Write("Stand (tryk Enter for at springe over): "); // Beder om stand
            string stand = (Console.ReadLine() ?? "").Trim(); // Læser input
            if (stand.ToUpper() == "A") return; // Går til hovedmenu hvis A
            if (stand.ToUpper() == "B") // Går til søgemenu hvis B
            {
                SøgEfterSpilMenu(spilListe); // Kalder søgemenuen
                return; // Afslutter metoden
            }

            Console.Write("Pris (tryk Enter for at springe over): "); // Beder om pris
            string pris = (Console.ReadLine() ?? "").Trim(); // Læser input
            if (pris.ToUpper() == "A") return; // Går til hovedmenu hvis A
            if (pris.ToUpper() == "B") // Går til søgemenu hvis B
            {
                SøgEfterSpilMenu(spilListe); // Kalder søgemenuen
                return; // Afslutter metoden
            }

            List<Spil> fundneSpil = new List<Spil>(); // Opretter en tom liste til de spil der matcher

            foreach (Spil spil in spilListe) // Går igennem hvert spil i listen
            {
                bool matcherTitel = string.IsNullOrWhiteSpace(titel) || spil.Titel.ToLower().Contains(titel.ToLower()); // Tjekker om titel matcher, eller om feltet er sprunget over
                bool matcherGenre = string.IsNullOrWhiteSpace(genre) || spil.Genre.ToString().ToLower().Contains(genre.ToLower()); // Tjekker om genre matcher
                bool matcherStand = string.IsNullOrWhiteSpace(stand) || spil.Stand.ToString().ToLower().Contains(stand.ToLower()); // Tjekker om stand matcher
                bool matcherPris = string.IsNullOrWhiteSpace(pris) || spil.Pris.ToString().Contains(pris); // Tjekker om pris matcher

                if (matcherTitel && matcherGenre && matcherStand && matcherPris) // Kun hvis alle valgte kriterier matcher
                {
                    fundneSpil.Add(spil); // Tilføjer spillet til listen over fundne spil
                }
            }

            Console.WriteLine(); // Tom linje for luft

            if (fundneSpil.Count == 0) // Hvis ingen spil blev fundet
            {
                Console.WriteLine("Ingen spil fundet."); // Besked til brugeren
            }
            else // Hvis der blev fundet resultater
            {
                Console.WriteLine("Fundne spil:"); // Overskrift
                SpilVisningService.PrintSpilTabel(fundneSpil); // Viser resultaterne i tabel-format
            }

            VentPåSøgningValg(spilListe); // Giver brugeren valg mellem søgemenu og hovedmenu efter søgningen
        }

        public static void VentPåSøgningValg(List<Spil> spilListe) // Hjælpemetode der spørger om brugeren vil tilbage til søgemenuen eller hovedmenuen
        {
            while (true) // Løkke indtil brugeren skriver et gyldigt valg
            {
                Console.WriteLine(); // Tom linje for luft
                Console.Write("Tast B for søgemenu eller A for hovedmenu: "); // Viser valgmuligheder
                string valg = (Console.ReadLine() ?? "").Trim().ToUpper(); // Læser input og gør det til store bogstaver

                if (valg == "A") // Hvis brugeren vælger A
                {
                    return; // Afslutter metoden og går tilbage til hovedmenuen
                }

                if (valg == "B") // Hvis brugeren vælger B
                {
                    SøgEfterSpilMenu(spilListe); // Kalder søgemenuen igen
                    return; // Afslutter metoden
                }
            }
        }
    }
}