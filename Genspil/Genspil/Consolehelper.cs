namespace Genspil // Angiver at klassen hører til i projektets namespace "Genspil"
{
    public static class ConsoleHelper // Opretter en statisk hjælpeklasse til fælles konsol-metoder
    {
        public static void Pause() // Metode der laver en pause, så brugeren kan nå at læse teksten i konsollen
        {
            Console.WriteLine(); // Skriver en tom linje for at skabe luft i konsollen
            Console.WriteLine("Tryk på en tast for at fortsætte..."); // Viser en besked til brugeren
            Console.ReadKey(); // Venter på, at brugeren trykker på en tast
        }

        public static void VentPåA(string promptTekst = "Indtast A for at vende tilbage: ") // Metode der bliver ved med at spørge, indtil brugeren skriver A
        {
            while (true) // Uendelig løkke som kun stoppes med return
            {
                Console.Write(promptTekst); // Viser teksten foran inputfeltet
                string input = (Console.ReadLine() ?? "").Trim().ToUpper(); // Læser brugerens input, undgår null, fjerner mellemrum og gør teksten til store bogstaver

                if (input == "A") // Tjekker om brugeren har skrevet A
                {
                    return; // Afslutter metoden og går tilbage til det sted, hvor metoden blev kaldt
                }
            }
        }
    }
}