namespace Genspil.Klasser
{
    // Hjælpeklasse med genbrugelige konsol-operationer
    public static class ConsoleHelper
    {
        // Viser en pause-besked og venter på at brugeren trykker en vilkårlig tast
        public static void Pause()
        {
            Console.WriteLine();
            Console.WriteLine("Tryk på en tast for at fortsætte...");
            Console.ReadKey();
        }

        // Venter indtil brugeren specifikt trykker 'A' for at fortsætte
        // Ignorerer alle andre tastetryk
        public static void VentPåA(string promptTekst = "Tryk A for at vende tilbage: ")
        {
            while (true)
            {
                Console.Write(promptTekst);
                char input = char.ToUpper(Console.ReadKey().KeyChar);

                if (input == 'A')
                {
                    return;
                }
            }
        }
    }
}